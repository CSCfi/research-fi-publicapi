#!/usr/bin/env python3
"""Merge Coverlet Cobertura/OpenCover reports and summarize test results."""

import argparse
import hashlib
import math
import sys
import xml.etree.ElementTree as ET
from collections import defaultdict
from datetime import datetime
from pathlib import Path


def percent(covered, total):
    return "n/a" if total == 0 else f"{covered / total * 100:.2f}%"


def parse_datetime(value):
    if not value:
        return None
    value = value.replace("Z", "+00:00")
    fraction_start = value.find(".")
    if fraction_start >= 0:
        timezone_start = next(
            (index for index in range(fraction_start + 1, len(value)) if value[index] in "+-"),
            len(value),
        )
        if timezone_start - fraction_start > 7:
            value = value[: fraction_start + 7] + value[timezone_start:]
    return datetime.fromisoformat(value)


def project_assemblies(source_root):
    assemblies = set()
    for project in source_root.rglob("*.csproj"):
        root = ET.parse(project).getroot()
        assembly_name = root.findtext(".//AssemblyName")
        if assembly_name:
            project_name = project.stem.replace(" ", "_")
            assembly_name = assembly_name.replace(
                '$(MSBuildProjectName.Replace(" ", "_"))', project_name
            ).replace("$(MSBuildProjectName)", project.stem)
        assemblies.add(assembly_name or project.stem)
    return assemblies


def unique_reports(results_root, pattern, expected_projects, label):
    reports_by_project = defaultdict(dict)
    for path in sorted(results_root.rglob(pattern)):
        project = path.relative_to(results_root).parts[0]
        digest = hashlib.sha256(path.read_bytes()).hexdigest()
        reports_by_project[project].setdefault(digest, path)
    if len(reports_by_project) != expected_projects:
        raise ValueError(
            f"Expected {expected_projects} {label} reports; found {len(reports_by_project)}"
        )
    reports = []
    for project, duplicates in sorted(reports_by_project.items()):
        if len(duplicates) != 1:
            raise ValueError(f"Found multiple distinct {label} reports for {project}")
        reports.append(next(iter(duplicates.values())))
    return reports


def report_projects(results_root, paths):
    return {path.relative_to(results_root).parts[0] for path in paths}


def collect_reports(results_root, expected_projects, production_assemblies):
    coverage_paths = unique_reports(
        results_root, "coverage.cobertura.xml", expected_projects, "Cobertura"
    )
    opencover_paths = unique_reports(
        results_root, "coverage.opencover.xml", expected_projects, "OpenCover"
    )
    trx_paths = sorted(results_root.rglob("*.trx"))
    discovery_paths = sorted(results_root.rglob("discovery.txt"))
    if len(trx_paths) != expected_projects:
        raise ValueError(f"Expected {expected_projects} TRX reports; found {len(trx_paths)}")
    if len(discovery_paths) != expected_projects:
        raise ValueError(
            f"Expected {expected_projects} test-discovery reports; found {len(discovery_paths)}"
        )
    project_sets = {
        frozenset(report_projects(results_root, paths))
        for paths in (coverage_paths, opencover_paths, trx_paths, discovery_paths)
    }
    if len(project_sets) != 1:
        raise ValueError("Coverage, test, and discovery reports do not cover the same projects")

    assembly_lines = defaultdict(dict)
    assembly_branches = defaultdict(dict)
    methods = {}
    source_line_count = 0

    for report_path in coverage_paths:
        root = ET.parse(report_path).getroot()
        report_line_count = 0
        for package in root.findall("./packages/package"):
            assembly = package.get("name", "")
            if assembly not in production_assemblies:
                continue
            for source_class in package.findall("./classes/class"):
                filename = source_class.get("filename", "")
                for line in source_class.findall("./lines/line"):
                    number = line.get("number")
                    if number is None:
                        continue
                    key = (filename, number)
                    hits = int(line.get("hits", "0"))
                    assembly_lines[assembly][key] = max(
                        hits, assembly_lines[assembly].get(key, 0)
                    )
                    report_line_count += 1
                for method in source_class.findall("./methods/method"):
                    method_lines = method.findall("./lines/line")
                    if not method_lines:
                        continue
                    method_key = (
                        assembly,
                        filename,
                        source_class.get("name", ""),
                        method.get("name", ""),
                        method.get("signature", ""),
                    )
                    entry = methods.setdefault(
                        method_key, {"lines": set(), "covered": set()}
                    )
                    for line in method_lines:
                        number = line.get("number")
                        if number is None:
                            continue
                        entry["lines"].add(number)
                        if int(line.get("hits", "0")) > 0:
                            entry["covered"].add(number)

        if report_line_count == 0:
            raise ValueError(f"Coverage report contains no source lines: {report_path}")
        source_line_count += report_line_count

    for report_path in opencover_paths:
        root = ET.parse(report_path).getroot()
        for module in root.findall(".//Module"):
            assembly = module.findtext("./ModuleName", "")
            if assembly not in production_assemblies:
                continue
            files = {
                file.get("uid", ""): file.get("fullPath", "")
                for file in module.findall("./Files/File")
            }
            for source_class in module.findall("./Classes/Class"):
                class_name = source_class.findtext("./FullName", "")
                for method in source_class.findall("./Methods/Method"):
                    method_name = method.findtext("./Name", "")
                    for branch in method.findall("./BranchPoints/BranchPoint"):
                        filename = files.get(branch.get("fileid", ""), "")
                        key = (
                            filename,
                            branch.get("sl", ""),
                            class_name,
                            method_name,
                            branch.get("offset", ""),
                            branch.get("path", ""),
                        )
                        assembly_branches[assembly][key] = (
                            int(branch.get("vc", "0")) > 0
                            or assembly_branches[assembly].get(key, False)
                        )

    if source_line_count == 0:
        raise ValueError("Coverage reports contain no instrumented source lines")
    return (
        coverage_paths,
        opencover_paths,
        trx_paths,
        discovery_paths,
        assembly_lines,
        assembly_branches,
        methods,
    )


def parse_discovery(discovery_paths):
    results = {}
    for path in discovery_paths:
        names = []
        listing_started = False
        for line in path.read_text(encoding="utf-8").splitlines():
            if "The following Tests are available:" in line:
                listing_started = True
            elif listing_started and line.strip():
                names.extend(line.split())
        if not names:
            raise ValueError(f"Test-discovery report contains no test names: {path}")
        results[path.parent.name] = len(names)
    return results


def parse_test_results(trx_paths):
    results = []
    for path in trx_paths:
        root = ET.parse(path).getroot()
        counters = root.find(".//{*}ResultSummary/{*}Counters")
        times = root.find(".//{*}Times")
        if counters is None:
            raise ValueError(f"TRX report has no result counters: {path}")
        total = int(counters.get("total", "0"))
        if total == 0:
            raise ValueError(f"TRX report contains no test cases: {path}")
        skipped = sum(
            result.get("outcome", "").lower() in {"notexecuted", "skipped"}
            for result in root.findall(".//{*}UnitTestResult")
        )
        start = parse_datetime(times.get("start")) if times is not None else None
        finish = parse_datetime(times.get("finish")) if times is not None else None
        duration = (finish - start).total_seconds() if start and finish else math.nan
        project = path.parent.name
        results.append(
            (
                project,
                total,
                int(counters.get("passed", "0")),
                int(counters.get("failed", "0")),
                skipped,
                duration,
            )
        )
    return results


def format_duration(seconds):
    return "n/a" if math.isnan(seconds) else f"{seconds:.2f}s"


def build_report(
    source_root,
    coverage_paths,
    opencover_paths,
    test_results,
    discovery_counts,
    assembly_lines,
    assembly_branches,
    methods,
):
    assemblies = sorted(
        assembly
        for assembly in project_assemblies(source_root)
        if not assembly.endswith(".DatabaseContext")
    )
    lines_by_assembly = {
        assembly: assembly_lines.get(assembly, {}) for assembly in assemblies
    }
    branches_by_assembly = {
        assembly: assembly_branches.get(assembly, {}) for assembly in assemblies
    }
    total_lines = sum(len(lines) for lines in lines_by_assembly.values())
    covered_lines = sum(
        sum(hits > 0 for hits in lines.values()) for lines in lines_by_assembly.values()
    )
    total_branches = sum(len(branches) for branches in branches_by_assembly.values())
    covered_branches = sum(
        sum(branches.values()) for branches in branches_by_assembly.values()
    )

    lines = [
        "# Test and coverage summary",
        "",
        f"Coverage reports merged by source line and branch outcome: "
        f"{len(coverage_paths)} Cobertura and {len(opencover_paths)} OpenCover reports.",
        "Generated DatabaseContext sources are excluded by `test/coverage.runsettings`.",
        "",
        "## Test results",
        "",
        "Listed entries come from `dotnet test --list-tests`; totals are runtime cases, "
        "which may include expanded theory data.",
        "",
        "| Project | Listed entries | Runtime cases | Passed | Failed | Skipped | Duration |",
        "| --- | ---: | ---: | ---: | ---: | ---: | ---: |",
    ]
    for project, total, passed, failed, skipped, duration in sorted(test_results):
        lines.append(
            f"| {project} | {discovery_counts[project]} | {total} | {passed} | {failed} | "
            f"{skipped} | {format_duration(duration)} |"
        )
    lines.extend(
        [
            "",
            "## Coverage",
            "",
            "| Production assembly | Lines | Branches |",
            "| --- | ---: | ---: |",
            f"| **Overall** | {covered_lines}/{total_lines} "
            f"({percent(covered_lines, total_lines)}) | "
            f"{covered_branches}/{total_branches} "
            f"({percent(covered_branches, total_branches)}) |",
        ]
    )
    for assembly in assemblies:
        assembly_lines_for_project = lines_by_assembly[assembly]
        assembly_branches_for_project = branches_by_assembly[assembly]
        covered = sum(hits > 0 for hits in assembly_lines_for_project.values())
        branches_covered = sum(assembly_branches_for_project.values())
        lines.append(
            f"| {assembly} | {covered}/{len(assembly_lines_for_project)} "
            f"({percent(covered, len(assembly_lines_for_project))}) | "
            f"{branches_covered}/{len(assembly_branches_for_project)} "
            f"({percent(branches_covered, len(assembly_branches_for_project))}) |"
        )

    uncovered_methods = []
    for (assembly, filename, class_name, name, signature), data in methods.items():
        total = len(data["lines"])
        uncovered = total - len(data["covered"])
        if total and uncovered:
            uncovered_methods.append(
                (
                    len(data["covered"]) / total,
                    -uncovered,
                    assembly,
                    filename,
                    min(int(line) for line in data["lines"]),
                    class_name,
                    name,
                    signature,
                    uncovered,
                    total,
                )
            )
    uncovered_methods.sort()

    lines.extend(
        [
            "",
            f"## Least-covered methods ({len(uncovered_methods)} methods with uncovered lines)",
            "",
            "| Rank | Assembly | Method | Source | Covered lines |",
            "| ---: | --- | --- | --- | ---: |",
        ]
    )
    for rank, item in enumerate(uncovered_methods[:100], start=1):
        (
            _rate,
            _uncovered_sort,
            assembly,
            filename,
            first_line,
            class_name,
            name,
            signature,
            uncovered,
            total,
        ) = item
        qualified_method = f"{class_name}.{name}{signature}"
        lines.append(
            f"| {rank} | {assembly} | `{qualified_method}` | "
            f"`{filename}:{first_line}` | {total - uncovered}/{total} |"
        )
    if not uncovered_methods:
        lines.append("| - | - | No methods with uncovered lines | - | - |")
    if len(uncovered_methods) > 100:
        lines.extend(
            [
                "",
                f"Showing the 100 least-covered methods of {len(uncovered_methods)}.",
            ]
        )
    return "\n".join(lines) + "\n"


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("results", type=Path, help="Root directory containing TRX and Cobertura files")
    parser.add_argument("--source-root", type=Path, required=True, help="Production projects root")
    parser.add_argument("--output", type=Path, help="Write the Markdown report to this path")
    parser.add_argument("--expected-projects", type=int, default=3)
    arguments = parser.parse_args()

    try:
        if not arguments.results.is_dir():
            raise ValueError(f"Results directory does not exist: {arguments.results}")
        if not arguments.source_root.is_dir():
            raise ValueError(f"Production projects directory does not exist: {arguments.source_root}")
        (
            coverage_paths,
            opencover_paths,
            trx_paths,
            discovery_paths,
            assembly_lines,
            assembly_branches,
            methods,
        ) = collect_reports(
            arguments.results,
            arguments.expected_projects,
            {
                assembly
                for assembly in project_assemblies(arguments.source_root)
                if not assembly.endswith(".DatabaseContext")
            },
        )
        test_results = parse_test_results(trx_paths)
        discovery_counts = parse_discovery(discovery_paths)
        report = build_report(
            arguments.source_root,
            coverage_paths,
            opencover_paths,
            test_results,
            discovery_counts,
            assembly_lines,
            assembly_branches,
            methods,
        )
        if arguments.output:
            arguments.output.parent.mkdir(parents=True, exist_ok=True)
            arguments.output.write_text(report, encoding="utf-8")
        else:
            print(report, end="")
    except (OSError, ET.ParseError, ValueError) as error:
        print(f"Coverage report generation failed: {error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
