Status: Phase 4 complete with accepted exceptions; Phase 5 not started
Status updated: 2026-11-10

# Test coverage improvement plan

## Current status (2026-11-10)

| Phase | Status | Summary |
| --- | --- | --- |
| 1. Observable baseline | Complete | Discovery, coverage reports, and inactive-test inventory recorded. |
| 2. Reliability and host isolation | Complete | Async tests and host-local configuration/authentication isolation repaired; service-free validation passed. |
| 3. Recover inactive scenarios | Complete with accepted exceptions | Elasticsearch and SQL Server recovery completed; P3-ORG and P3-TIME remain documented exceptions. |
| 4. High-risk branch and contract coverage | Complete with accepted exceptions | Service-free suite: 494 passed, 0 failed, 0 skipped. Representative populated API contracts accepted. GitHub CI, including Elasticsearch and SQL Server lanes, was reported passed by the user; run details were not independently retrieved. Eight findings remain in the follow-up plan for review by 2026-12-10. |
| 5. Sustainable coverage and CI gates | Not started | Separate follow-on phase; not required for Phase 4 acceptance. |

**Next action:** start Phase 5 when authorized. Track the eight accepted findings
in the [defect follow-up plan](test-coverage-defect-follow-up-plan.md); their
review deadline does not block Phase 4 completion.

The detailed progress notes below are chronological. Any older statement that a
phase remains in progress records the status at that point, not the current
status summarized here.

## Goal and scope

Make existing tests trustworthy, recover useful disabled scenarios, and add risk-focused coverage across the SQL Server -> repositories -> indexer -> Elasticsearch -> HTTP API pipeline.

Production code must not be changed during implementation of this plan, except
to remove confirmed unused code. Do not fix defects, refactor production code for
testability, or change production behavior to make tests pass. Preserve
authorization policies, intentionally hidden endpoints, V2 documentation
visibility, and current public contracts. Removing unused code is not permission
to remove supported behavior.

Record defects discovered by test improvements as findings in the separate
[production defect follow-up plan](test-coverage-defect-follow-up-plan.md).
Each finding needs evidence, impact, a reproducing test or test gap, and the
decision/validation needed for a future fix. Fix implementation belongs to a
separately approved workstream, not a later phase of this coverage plan.
Characterization tests may document current failures but must not present them as
the desired contract. If a desired-behavior test cannot pass without a production
fix, link its tracked exception to the finding rather than weakening assertions
or changing production code. External-service provisioning remains subject to
the separate review described below.

Exporter coverage is excluded because the Exporter project is planned for removal
from the solution. Do not add exporter tests or test-project references; this
exclusion does not apply to the separately maintained logging formatters.

## 1. Findings from source inspection

### Phase 1 baseline (2026-11-09)

The initial baseline was captured before changing test or application code. Existing .NET 10 test and Coverlet packages successfully discovered and ran all projects and produced Cobertura reports, so no package upgrade was needed.

| Test project | Listed test entries | Runtime cases | Passed | Failed | Skipped | TRX run duration |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Interface.Tests | 47 | 47 | 32 | 0 | 15 | 14.24s |
| Indexer.Tests | 25 | 25 | 25 | 0 | 0 | 12.91s |
| Repository.Tests | 7 | 7 | 7 | 0 | 0 | 3.10s |
| **Total** | **79** | **79** | **64** | **0** | **15** | **30.25s** |

Listed entries and runtime cases are reported separately so theory data expansion is visible; these measurements happened to match. The 15 skipped runtime cases correspond to the skipped methods in the inventory below; three commented-out scenarios remain undiscovered.

Merged line coverage is calculated by source-line identity from Cobertura, and branch coverage by individual branch outcome from OpenCover; neither metric averages project percentages. `DatabaseContext/**/*.cs` is excluded as generated code; other production assemblies, including handwritten repositories and query generators, remain in scope.

| Production assembly | Covered lines | Covered branches |
| --- | ---: | ---: |
| **Overall** | **2471/6535 (37.81%)** | **273/1504 (18.15%)** |
| ApiModels | 234/505 (46.34%) | 6/14 (42.86%) |
| CSC.PublicApi.ElasticService | 154/1383 (11.14%) | 23/632 (3.64%) |
| CSC.PublicApi.Exporter | n/a (no instrumented lines) | n/a |
| CSC.PublicApi.Indexer | 10/314 (3.18%) | 2/62 (3.23%) |
| CSC.PublicApi.Interface | 534/1670 (31.98%) | 47/168 (27.98%) |
| CSC.PublicApi.Logging | 0/171 (0.00%) | 0/42 (0.00%) |
| CSC.PublicApi.Repositories | 1061/1957 (54.22%) | 156/536 (29.10%) |
| CSC.PublicApi.Service.Models | 478/535 (89.35%) | 39/50 (78.00%) |

No baseline test failures occurred. The generated CI report includes the 100 least-covered of 721 methods with uncovered lines and the full per-project results at `aspnetcore/TestResults/coverage-summary.md`.

### Existing foundation

- [PublicApi.sln](../aspnetcore/PublicApi.sln) includes all three test projects: [Interface.Tests](../aspnetcore/test/Interface.Tests/Interface.Tests.csproj), [Indexer.Tests](../aspnetcore/test/Indexer.Tests/Indexer.Tests.csproj), and [Repository.Tests](../aspnetcore/test/Repository.Tests/Repository.Tests.csproj).
- Tests use xUnit and FluentAssertions; Moq, WebApplicationFactory, an EF InMemory package, and an in-memory Elasticsearch transport are already available in Interface.Tests.
- Active tests cover several mapping profiles, repository in-memory transformations, URL generation, index-name configuration, middleware, Swagger V1, and three index-rotation success paths. Profile controller/client tests currently cover one backend-400 scenario each.
- [CI](../.github/workflows/dotnet.yml) restores, builds, records test discovery, collects coverage, and uploads reports on PRs to `devel`/`main`.
- All three test projects reference `coverlet.collector`; existing versions were verified with .NET 10, so no Test SDK or collector upgrade was necessary.

### Inactive test inventory and disposition

At baseline there were **15 explicitly skipped methods**: ten GET theories, one POST fact, and four repository debugging facts. The GET theories now have synthetic Elasticsearch replacements, with the two organization cases explicitly deferred after revealing a production field-name mismatch. The four obsolete repository debugging facts have been retired. The maintainer confirmed funding calls are GET-only: the unsupported no-op POST and its service method were removed, and its skipped persistence test was replaced by active HTTP 405 cases. The obsolete commented publication mapping examples have been replaced by active relationship tests, and the old V2 Swagger expectation has been replaced by an active test of its intentional non-publication.

| Location | Inactive methods | Problem / proposed disposition |
| --- | ---: | --- |
| [FundingCallSearchSystemTest](../aspnetcore/test/Interface.Tests/ElasticsearchSearchSystemTest.cs) | 2 recovered cases; unsupported POST retired | Replaced live-data GET theories with combined synthetic name/foundation/business-ID/category/date matching and an exact-business-ID negative control at `v1/funding-calls`. Active POST rejection and Swagger tests enforce the maintainer-confirmed GET-only contract. |
| [FundingDecisionSearchSystemTest](../aspnetcore/test/Interface.Tests/ElasticsearchSearchSystemTest.cs) | 2 recovered cases | Replaced hard-coded/live-data expectations with a synthetic combined name/funder/project/type match and a negative type-of-funding control using the public response model. |
| [InfrastructureSearchSystemTest](../aspnetcore/test/Interface.Tests/ElasticsearchSearchSystemTest.cs) | 2 recovered cases | Replaced old route and always-true predicate with synthetic `infraName` positive and negative controls, asserting exact count and identifier/name fields. |
| [OrganizationSearchSystemTest](../aspnetcore/test/Interface.Tests/ElasticsearchSearchSystemTest.cs) | 2 cases quarantined pending production review | Live Elasticsearch exposed that `OrganizationQueryGenerator` searches lowercase field names while the configured index maps PascalCase model properties. Both positive and negative integration cases are explicitly skipped until a separate production fix is reviewed; endpoint remains intentionally hidden from Swagger. |
| [ResearchDatasetSearchSystemTest](../aspnetcore/test/Interface.Tests/ElasticsearchSearchSystemTest.cs) | 2 recovered V1 cases plus V2 contract | Uses seeded V1 responses and separately exercises the V2 HTTP response without publishing V2 Swagger. |
| FundingCallRepositorySystemTest (retired) | 1 debugging fact | The placeholder name and API-host-coupled query only checked for a developer-specific live row. Replaced with four [SQL Server tests](../aspnetcore/test/Repository.Tests/FundingCallSqlServerTest.cs) against a disposable container and synthetic query-surface schema; in-memory enrichment remains covered separately. |
| FundingDecisionRepositorySystemTest (retired) | 3 debugging facts | Real record names/acronyms and weak/manual assertions were replaced with synthetic funding-decision SQL tests covering EU/PIC eligibility, related projection, enrichment and paging, plus in-memory consortium tests. Null-timestamp projection failures are explicitly characterized for separate production review. |
| [Indexer publication mapping tests](../aspnetcore/test/Indexer.Tests/Maps/PublicationProfileTest.cs) | 2 obsolete commented facts removed | Active tests cover parent/child IDs, multiple organization parts and their contributions, and absent relationships. Repository tests cover duplicate contribution references, missing cached organizations/unit parents, absent contributions, and organization-part URL suppression. |
| [SwaggerSystemTest](../aspnetcore/test/Interface.Tests/SwaggerSystemTest.cs) | 1 commented V2 fact | Replaced with active assertions that V2 Swagger and intentionally hidden organization paths are absent; V2 HTTP behavior is tested separately. |

### Reliability problems to fix before growing test counts

1. [FundingCallControllerTest](../aspnetcore/test/Interface.Tests/FundingCallControllerTest.cs) does not await controller calls. Asserting a non-null `Task` and verifying a dependency call cannot prove successful completion or valid response headers.
2. Skipped GET tests use `async void` and resolve `ApiDbContext` even though API-host registration of that context/repositories is commented out in [Program](../aspnetcore/src/Interface/Program.cs). Search tests must not require SQL Server.
3. [TestWebApplicationFactory](../aspnetcore/test/Interface.Tests/TestWebApplicationFactory.cs) changes process-wide environment variables and points Elasticsearch at localhost; it does not replace outbound services. This risks configuration leakage between parallel tests and accidental network access.
4. [TestAuthHandler](../aspnetcore/test/Interface.Tests/TestAuthHandler.cs) always authenticates with every policy role. Existing happy-path tests cannot prove 401/403 enforcement.
5. [MockElasticConnection](../aspnetcore/test/Interface.Tests/TestHelpers/MockElasticConnection.cs) returns generic successful bodies for unhandled requests. That is useful for request inspection, but cannot prove real search matching, bulk-item success, or cluster health semantics.
6. At baseline, the funding-call POST service method performed no persistence. Phase 3 resolved the contract as GET-only and removed the unsupported write path rather than treating no-op success as correct behavior.

## 2. Test design

Use the existing test projects and helpers first. Keep repository projection tests in their present project unless moving them directly improves ownership; avoid structural churn merely to increase coverage.

| Test tier | Scope | Dependencies | Execution |
| --- | --- | --- | --- |
| Unit | Mapping, query serialization, pagination, transformations, index orchestration, HTTP backend client | Synthetic objects, Moq, controlled HTTP/NEST transport | Required PR job; no external services |
| API integration | Routing, binding, auth policies, serialization, middleware, response headers, DI | WebApplicationFactory with explicit service replacements | Required PR job; no external services |
| Elasticsearch integration | Actual filter/analyzer/nested-query behavior, indexing and alias switching | Disposable, version-pinned Elasticsearch with production-compatible mappings | Separate required PR job after stabilization |
| SQL Server integration | EF translation, relational joins/projections, chunk ordering and enrichment inputs | Isolated SQL Server test database and synthetic seed data | Separate required PR job after provisioning is proven |

Use xUnit `Category=ElasticsearchIntegration` and `Category=SqlServerIntegration` traits for service-dependent tests. Untagged tests remain in the fast lane. A selected integration lane must fail if its service is unavailable, rather than silently skipping or falling back to a fake.

Mocks prove outgoing request shape and application control flow, not Elasticsearch matching or EF SQL translation. Neither EF InMemory nor LINQ-to-Objects is sufficient evidence of SQL Server behavior.

## 3. Implementation phases

### Phase 1 - Establish an observable baseline (P0, 1-2 developer-days)

**Dependencies:** none.

- [x] Discover tests from the solution and record executed/passed/failed/skipped counts per project, with listed entries and runtime cases reported separately.
- [x] Capture current failures before changing tests. No failures occurred; the existing skips are recorded above.
- [x] Collect Cobertura coverage using the existing collector. Existing .NET 10 package versions worked without changes.
- [x] Add a shared runsettings file and CI artifact upload for TRX and coverage, including results on failed runs. Add a repository-local report/merge step only if needed; no third-party code-upload service.
- [x] Merge coverage from all test projects by covered source lines/branches, not by averaging percentages; shared production assemblies are exercised by multiple projects.
- [x] Publish overall and per-production-assembly line/branch coverage. Exclude test assemblies, build outputs, and scaffolded DatabaseContext code. Keep handwritten repositories, query generators, middleware, and orchestration included. Document any other generated-code exclusions narrowly.
- [x] Produce the ranked uncovered-method list, suite durations, and an explicit inactive-test disposition checklist based on section 1.

**Acceptance:** reproducible discovery and coverage reports for all three projects; no empty report accepted as success; runtime percentages and unresolved failures are recorded without claiming an unmeasured baseline.

### Phase 2 - Repair test reliability and host isolation (P0, 2-3 days)

**Dependencies:** phase 1 baseline.

- [x] Rewrite the four funding-call pagination tests as `async Task`; configure meaningful search results, assign an HTTP context, await completion, and assert returned values and headers as well as arguments.
- [x] Convert touched `async void` tests to `async Task`; dispose clients/scopes/responses appropriately.
- [x] Replace process-wide test configuration mutation with host-local configuration. Explicitly replace search/profile clients and any database services used by a test. Keep test secrets synthetic and prevent real Elasticsearch, Keycloak, SQL Server, profile-backend, and logging-sink calls in the fast lane.
- [x] Make test authentication selectable: anonymous, authenticated without required role, and authenticated with exactly the required role. Include client identity claims where relevant.
- [x] Extend the existing NEST test transport with realistic, scenario-specific search/bulk/health responses. Fail explicitly on unexpected requests in new tests; preserve and then strengthen existing index-rotation assertions.
- [x] Check Scrutor and typed HttpClient registration through the test host, not only manually constructed controllers.

**Acceptance:** fast tests pass without running external services or using developer secrets; a deliberately faulted async dependency fails its test; parallel test hosts do not leak configuration/auth state. All four pagination cases assert completed results.

**Validation (2026-11-09):** the full solution suite passed with 67 passed, 15 intentionally skipped, and 0 failed. The test-host isolation tests passed with anonymous, authenticated, and role-bearing hosts issuing requests concurrently (2 tests passed); `git diff --check` passed. The skipped funding-call POST remains an explicit phase 3/API-owner decision.

### Phase 3 - Recover inactive scenarios safely (P1, 3-5 days)

**Status: Complete with accepted exceptions (2026-11-09).** The maintainer
reported three successful CI runs after pushing the changes and opening a PR.
The agreed stability gate is three runs, replacing the original conservative
ten-run proposal. Parallel isolation was verified locally, not by sequential CI
reruns. Monitor subsequent PRs for intermittent failures.

**Dependencies:** phase 2; fixture/provisioning work can proceed independently for Elasticsearch and SQL Server.

#### Elasticsearch search recovery

- [x] Run the Testcontainers-backed Elasticsearch 8.19.22 lane successfully three consecutive times in CI, with separate local parallel-isolation evidence. Maintainer-reported CI results satisfy the revised gate; required-check configuration is a separate repository setting.
- [x] Seed synthetic documents through the production `AutoMap` mapping, with unique test-owned index names and host configuration; explicitly refresh before searching.
- [x] Replace the ten skipped GET theories with active tests on current routes/public response types; remove unnecessary `ApiDbContext` resolution.
- [x] Preserve representative name/funder/business-ID/category/date/type-of-funding cases using current parameter names and query boundaries.
- [x] Include matching documents, near misses, and combined-filter counterexamples. Assert exact counts and public fields rather than using `OnlyContain`.
- [x] Replace the always-true infrastructure predicate with field-level assertions and a negative control.
- [x] Record the maintainer-approved organization-search exception (P3-ORG below); positive/negative cases remain skipped until a separately reviewed production correction.
- [x] Cover V2 dataset behavior as an HTTP contract distinct from documentation visibility.
- [x] Add publication search/detail and infrastructure-service integration scenarios.

#### Repository and publication recovery

- [x] Retire the four developer-only debugging facts; retain the synthetic funding-call enrichment test and add a funding-decision consortium-enrichment assertion in Repository.Tests. True relational retrieval remains dependent on the isolated SQL lane below.
- [x] Build only the SQL schema surface needed for initial funding-call repository scenarios using the approved scaffolded mappings and versioned [FundingCallSchema.sql](../aspnetcore/test/Repository.Tests/SqlServer/FundingCallSchema.sql). Scaffolded entities are not migrations: do not assume `EnsureCreated` can reproduce the warehouse or hand-edit generated entities.
- [x] Run funding-call relational queries against an isolated SQL Server database; supply required reference/sentinel rows and relationships explicitly. No production records or credentials are used.
- [x] Extend the initial SQL schema and relational query coverage to funding decisions, including EU/PIC consortium eligibility.
- [x] Record the maintainer-approved null-timestamp exception (P3-TIME below). Nullable database `created`/`modified` projected to non-nullable model timestamps produces SQL error 242; characterization tests preserve evidence without changing production behavior.
- [x] Verify the new SQL Server CI lane on Linux x86-64 with repeated clean runs: the maintainer reported three successful CI runs.
- [x] Add synthetic publication relationship mapping coverage for parent/child IDs and organization/co-publication flags, plus post-processing coverage for duplicate contribution references.
- [x] Complete publication post-processing coverage for missing relationships and remove the obsolete commented mapping examples.
- [x] Replace commented V2 Swagger code with assertions of its intended non-published behavior; the V1 path matrix continues to assert intentionally hidden paths remain absent.
- [x] Resolve funding-call POST: the maintainer confirmed only GET is supported. Remove the unsupported no-op POST and service method; replace the skipped persistence case with HTTP 405 checks for anonymous, authenticated, and role-bearing callers, and assert Swagger publishes only GET.

 both service lanes use Testcontainers with dynamically assigned ports, independently owned containers, and synthetic data. Elasticsearch remains pinned to the maintainer-confirmed 8.19.22 image, with two organization searches deferred for separate production review. Its fixture now provisions all seven indices, including five synthetic publications and three services, and verifies exact seeded counts. The 32 new publication/service cases cover combined and nested filters, correlated author names, exact identifiers, publication visibility/detail and missing IDs, year boundaries, stable ordering, and pagination totals. The disabled infrastructure-service detail route remains unchanged. The SQL Server 2022 CU27 fixture applies versioned, minimal funding-call and funding-decision schemas derived from the approved scaffolded mappings; it does not use `EnsureCreated` or developer databases. Four funding-call cases cover eligibility, dates/sentinels, split-query relationships, language/type-specific links, and source-row chunk boundaries. Eight funding-decision cases cover EU/PIC eligibility (including funder-only/unrelated PIC and wrong PID-type counterexamples), domestic/null sources, multiple PICs and mixed consortium membership, self/parent contributors, programme ancestors, projection/enrichment, and paging. Two of those eight characterize existing failures for null `created`/`modified` timestamps using rolled-back transactions; timestamp production behavior is unchanged. Both integration CI lanes collect TRX/coverage. [Test documentation](../aspnetcore/test/README.md) records setup, cleanup, schema scope, and platform/tooling limitations. Publication relationship and API scenario recovery are complete. The close-out below records the accepted defect exceptions and resolved GET-only funding-call contract; Linux CI observation remains open.

**Validation (2026-11-09):** the service-free solution lane passed with 78 passed, 1 intentionally skipped POST, and 0 failed. The expanded Elasticsearch lane passed with 41 passed and 2 deferred organization skips, both with CI's coverage/TRX options and in two concurrent non-coverage processes using distinct containers; all test-owned Elasticsearch containers were removed. All 12 SQL Server integration cases previously passed with CI's coverage/TRX options and in two concurrent non-coverage processes, with cleanup verified. An initial SQL concurrency attempt overlapped Coverlet instrumentation of shared DLLs with another run and caused `BadImageFormatException` in that run; rerunning isolation without concurrent instrumentation passed. The local Docker host is ARM64 using x86-64 emulation, not an officially supported SQL Server platform, so Linux x86-64 CI validation remains outstanding. Workflow YAML previously parsed with Ruby's standard parser; the local Python interpreter is missing `libintl`. `git diff --check` passed.

The progress and local-validation notes above describe the pre-CI state. The
close-out below supersedes their outstanding-CI statements.

#### Close-out decisions and CI handoff (2026-11-09)

**Phase 3 is complete with accepted exceptions and maintainer-reported CI acceptance.**
The maintainer explicitly retained the organization and timestamp defects for separate
production review and confirmed that funding calls support only GET. Removing the
unsupported no-op POST is the only production behavior change in this close-out;
GET, export, and their authorization policies are unchanged.

| Tracking ID | Disposition and impact | Owner and review checkpoint |
| --- | --- | --- |
| P3-ORG | Accepted Phase 4 coverage exception: the two organization integration cases stay skipped because query field names do not match the production mapping. This does not certify organization search as working. Tracked in the [follow-up plan](test-coverage-defect-follow-up-plan.md#p3-org---organization-search-field-name-mismatch); no production fix is in scope here. | Repository maintainers; review by 2026-12-10. |
| P3-TIME | Accepted Phase 4 coverage exception: null funding-decision timestamps still cause SQL error 242. The two transaction-isolated characterization cases remain active; no fallback timestamp contract is invented. Tracked in the [follow-up plan](test-coverage-defect-follow-up-plan.md#p3-time---funding-decision-null-timestamp-projection); no production fix is in scope here. | Repository maintainers; review by 2026-12-10. |
| P3-CI | Closed under the revised three-run gate: the maintainer reported three successful CI runs. Both integration lanes passed in Linux CI; parallel-isolation evidence comes from the earlier local runs. Continue observing subsequent PRs. | Repository maintainers; monitor intermittent failures. Required-check settings have not been changed by this work. |

The maintainer committed/pushed the implementation and opened a PR, then reported
three successful CI runs. These are maintainer-reported results, not independently
retrieved run logs. The original ten-run proposal was reduced to three as a
proportionate repeatability check, not a statistical guarantee of reliability.
No branch-protection change is performed here.

**Close-out validation:** 14 targeted tests passed, with no failures or skips,
covering POST rejection, Swagger visibility, funding-call controller pagination,
and isolated host authentication/service registration. Earlier full-lane counts
above predate the replacement of the skipped POST case and addition of the GET-only
Swagger test; they are retained as historical measurements, not new suite totals.

**Acceptance met with exceptions:** all remaining inactive scenarios have a recorded disposition (active replacement, intentional retirement, or owned/time-bounded exception). The Elasticsearch lane passes its enabled synthetic fixture scenarios; organization searches remain explicitly deferred under P3-ORG. Funding-call POST is retired and rejected with 405 rather than returning no-op success. Failed setup is not reported as a successful/empty integration run. P3-CI is satisfied by the maintainer-reported results under the revised gate. P3-ORG and P3-TIME are accepted Phase 4 exceptions with a 2026-12-10 review date.

### Phase 4 - Expand high-risk branch and contract coverage (P1/P2, 4-6 days)

**Dependencies:** phase 2 for unit/API work; phase 3 fixtures for real-service cases. Start the highest-risk items before waiting for every recovery task.

**Progress (2026-11-10):** started with the serialized query-generator contract
suite in [QueryGeneratorContractTest](../aspnetcore/test/Interface.Tests/QueryGenerators/QueryGeneratorContractTest.cs).
It resolves all seven generators and the NEST client through production
`AddElasticSearch` registration and serializes descriptors without network calls.
The 153 new runtime cases cover configured indices, defaults, first/later pages,
stable search/export sorting, absent/zero/large export cursors, count/export filter
parity, individual filter clauses and nested paths, required combined filters,
correlated author names, whitespace controls, inclusive dates/years, open-ended
funding calls, missing funding-decision start dates, publication visibility,
dataset latest-version selection, and the three implemented public-ID lookups.
Organization name-search semantics remain excluded under P3-ORG; its shared
paging/export/default behavior is covered without endorsing the defective query.

**Validation (2026-11-10):** the new contracts and existing Elasticsearch
integration lane passed together: 194 passed, 2 existing organization skips,
0 failures. This includes all 153 new cases and 41 representative real-service
cases from Phase 3. Test-owned containers were cleaned up. Production code was
not changed, and no new confirmed defect was discovered in this tranche.
This completes the initial serialized query-contract tranche, not Phase 4 as a
whole or exhaustive Elasticsearch semantics for every filter.

**Search-service progress (2026-11-10):** added 27 active runtime cases in
[ElasticSearchServiceTest](../aspnetcore/test/Interface.Tests/Services/ElasticSearchServiceTest.cs).
A strict scripted transport exercises the real NEST serializer/deserializer and
production publication query generator without external services. Tests cover
empty/single/multiple hits, result metadata and page counts, missing hit metadata,
single lookup and absent documents, full/partial/empty export traversal without
duplicate or omitted documents, zero/large long cursors, last-hit sort selection,
counts above 10,000, exact search/count filter parity and request order, and HTTP
400/503 failures with default and throwing client settings. Successful scripted
responses must also have successful transport status; unexpected requests and
unconsumed responses fail the tests.

**Search-service validation (2026-11-10):** the Interface service-free suite
passed with 221 passed, 0 skipped, 0 failed, including all 153 query contracts and
27 new search-service cases. No production files were changed. Tests confirmed
two findings, recorded separately as
[P4-SEARCH](test-coverage-defect-follow-up-plan.md#p4-search---failed-elasticsearch-searches-look-like-empty-results)
and [P4-COUNT](test-coverage-defect-follow-up-plan.md#p4-count---export-count-failures-return-misleading-or-suppressed-totals).
Characterization cases preserve their evidence without endorsing silent failures;
production fixes remain out of scope. Next: index-service boundaries and failure
transitions, followed by the remaining matrix below. Phase 4 remains in progress.

**Index-service progress and validation (2026-11-10):** added 29 runtime cases in
[ElasticSearchIndexBoundaryTest](../aspnetcore/test/Interface.Tests/Services/ElasticSearchIndexBoundaryTest.cs),
alongside the three existing rotation tests. Full indexing and chunk indexing
cover 0/1/1499/1500/1501 documents, exact NDJSON document order and uniqueness,
batch sizes, and request ordering. Tests verify alias targets and health wait
parameters, missing index-name rejection before requests, failed/unacknowledged
creation, first/second-batch item and HTTP failures, error logging, and prevention
of further batches or activation after bulk failure. Chunk writes never activate
an index. Empty full indexing currently still rotates; this is observed existing
behavior, not a new policy decision.

Lifecycle characterization covers failed or timed-out health checks,
failed/unacknowledged alias changes, failed/allowed-404 old-index deletion,
failed pre-deletion, and failed existence checks. These exposed
[P4-ROTATE](test-coverage-defect-follow-up-plan.md#p4-rotate---unchecked-index-lifecycle-responses-permit-unsafe-transitions),
recorded for separate follow-up without changing production code.
The Interface service-free suite passed with 250 passed, 0 skipped, 0 failed.
Retries are disabled only in the synthetic test transport; no Docker is needed.
Next: API contract/authorization and profile-backend coverage, followed by the
remaining repository/indexer matrix. Phase 4 remains in progress.

**Profile progress and validation (2026-11-10):** added 30 runtime cases across
[ProfileBackendContractTest](../aspnetcore/test/Interface.Tests/Services/ProfileBackendContractTest.cs),
[ProfileEndpointContractTest](../aspnetcore/test/Interface.Tests/ProfileEndpointContractTest.cs),
and [ProfileControllerTest](../aspnetcore/test/Interface.Tests/Controllers/ProfileControllerTest.cs).
They exercise the actual POST path/body, success deserialization, configured
synthetic token via typed-client DI, optional client-ID forwarding, exact controller
request/RequestAborted forwarding, in-flight client cancellation, network errors,
backend error status/body/content-type preservation, malformed/empty success JSON,
HTTP 401/403/200 role boundaries, and rejection of empty/malformed/unsupported
request bodies before backend calls. All outbound requests use test handlers.
The existing controller-400 case plus new cases passed (31 targeted tests);
the Interface service-free suite passed with 280 passed, 0 skipped, 0 failed.
No production code was changed. JSON null backend success was characterized at
both client and HTTP layers and recorded as
[P4-PROFILE](test-coverage-defect-follow-up-plan.md#p4-profile---null-profile-backend-success-becomes-http-204).
Broader API route/policy, pagination/export, and repository/indexer coverage remain
open; this tranche does not complete Phase 4 or certify every profile data shape.

**API route and pagination progress (2026-11-10):** added 111 runtime cases in
[ApiRoutePolicyTest](../aspnetcore/test/Interface.Tests/ApiRoutePolicyTest.cs),
[FundingCallHttpContractTest](../aspnetcore/test/Interface.Tests/FundingCallHttpContractTest.cs),
and [ResponseHelperContractTest](../aspnetcore/test/Interface.Tests/ResponseHelperContractTest.cs).
The matrix tests 24 route/method combinations in anonymous, authenticated/no-role,
wrong-role, and correct-role modes. It includes all search/export resources,
hidden organization/V2 routes, missing publication detail, all four Virta routes,
and funder read/POST/PUT/DELETE policies. Search responses are isolated fakes;
Virta/import contexts use uniquely named EF InMemory stores, not SQL servers.
Funder POST verifies actual test-store persistence and that its write role alone
cannot read the created Location. Missing funder detail/update/delete cases
return 404 for the correct role. Successful funder PUT and DELETE cases now
verify persisted changes and client isolation in the EF InMemory store.
The two successful mutation cases passed as part of the 98-case focused
ApiRoutePolicyTest suite.

Funding-call HTTP contracts follow export links across full, partial, and empty
pages with exact cursor/filter forwarding and no duplicate or omitted records,
assert public JSON omits internal sort IDs, test query binding and oversized-page
normalization, and reject malformed query values before dependency calls.
Helper tests assert first/middle/last link relations, repeated/escaped filters,
inclusive navigation targets, zero/large export cursors, final-page headers,
and Virta pagination. Empty pagination exposed
[P4-LINK](test-coverage-defect-follow-up-plan.md#p4-link---empty-pagination-links-to-page-zero),
recorded without a production fix.

**Validation:** Interface service-free suite: 391 passed, 0 skipped, 0 failed.
Production code is unchanged. The broader route matrix supplements the separate
profile matrix; it does not certify SQL translation, every resource's query/JSON
shape, populated Virta output, or complete funder CRUD/client-isolation semantics.
Those and repository/indexer coverage remain follow-on Phase 4 work.

**Repository post-processing progress (2026-11-10):** added six cases across
[ResearchDatasetIndexRepositoryTest](../aspnetcore/test/Repository.Tests/ResearchDatasetIndexRepositoryTest.cs)
and [InfrastructureServiceIndexRepositoryTest](../aspnetcore/test/Repository.Tests/InfrastructureServiceIndexRepositoryTest.cs).
They cover all three dataset contributor-organization relationship paths, removal
of contributors without a person or real organization, infrastructure-service
URN/other-PID normalization, Research.fi link generation, and empty PID handling.
The identifier/link behavior is asserted through both single-entity and bulk
post-processing. The targeted repository tests passed (10 total); the complete
service-free Repository.Tests lane passed (20 passed, 0 skipped, 0 failed).
No production code changed and this tranche exposed no new defect. These unit
cases do not cover SQL projection or populated database-backed
infrastructure-service relationships; Phase 4 remains in progress.

**Indexer orchestration progress (2026-11-10):** added eight cases in
[IndexerTest](../aspnetcore/test/Indexer.Tests/IndexerTest.cs). They cover small
empty/nonempty full-index inputs, publication chunking at 14,999/15,000/15,001
documents, exact document uniqueness/order, final empty-chunk requests, and
repository retrieval failure. A recording logger verifies the error is surfaced;
the failed index is not activated, and processing continues to the next configured
type. Additional tests verify organization and funding-call cache population
before processing, source-ID and grouped programme entries, sentinel handling,
and logging/continuation when a configured repository is missing. Targeted
indexer tests passed (8). Repository tests now also cover organization-cache
hits/misses in infrastructure post-processing, through both single and bulk
paths; the focused repository suite passed (13). SQL-backed
infrastructure-service enrichment and the infrastructure repository projection
are now covered by relational integration cases. The full SQL lane passed; Phase 4
remains in progress pending remaining P1 acceptance gaps.
Phase 4 is not complete.

**Preflight threshold progress (2026-11-10):** expanded
[DatabasePreflightCheckTest](../aspnetcore/test/Indexer.Tests/Preflight/DatabasePreflightCheckTest.cs)
to cover below, exactly at, and above the 80% threshold, including a zero-count
helper characterization. The test documents the helper result only; it does not
assert that an empty publication table is acceptable, since `IsGood` has a
separate empty-table check. Added SQL Server integration cases in
[DatabasePreflightCheckSqlServerTest](../aspnetcore/test/Repository.Tests/DatabasePreflightCheckSqlServerTest.cs)
for the full `IsGood` query, exact 80% result, sentinel filtering, empty required
tables, and failure/status logging, backed by a minimal preflight query-surface
schema in the shared test fixture. The targeted preflight integration tests passed
(3), and all SQL Server integration tests passed (16) with Docker running. The
Repository.Tests service-free lane also passes (23).

**Infrastructure-service SQL enrichment progress (2026-11-10):** added
[InfrastructureServiceSqlServerTest](../aspnetcore/test/Repository.Tests/InfrastructureServiceSqlServerTest.cs)
and a minimal test-only query schema. The test exercises the repository's real
SQL lookups for infrastructure PIDs, participant facts, responsible-organization
IDs, names, and ESFRI references, then asserts cache-backed organization details
and generated links. Duplicate participants are collapsed and unrelated
contribution types excluded; inserts are transaction-isolated. The test passed,
and the full SQL Server integration lane passed (16).

**Infrastructure SQL projection progress (2026-11-10):** added
[InfrastructureSqlServerTest](../aspnetcore/test/Repository.Tests/InfrastructureSqlServerTest.cs)
for the real `InfrastructureIndexRepository.GetAll` projection and subsequent
post-processing. It verifies date and descriptive-name projection, responsible
and participating organization mapping, cached identifier enrichment, URN/link
generation, and filtering of sentinel/`infra_old` rows. The minimal test-only
schema covers the selected query surface, not the warehouse schema. The focused
case passed; the complete SQL Server integration lane passed (17), and the
service-free solution lane passed (471). No defect was exposed. Phase 4 remains
in progress pending remaining P1 acceptance gaps.

**Middleware progress (2026-11-10):** expanded the correlation-ID and global
error-handler tests to cover actual response headers for generated and supplied
IDs, context propagation, sanitized JSON payloads, the missing-ID fallback, and
the combined middleware pipeline's matching response header/body correlation ID.
All eight targeted middleware cases passed. Tests use an in-memory ASP.NET Core
server and synthetic exceptions; no production code changed. Logging sink
formatters and the remaining mapping edge cases are not covered by this tranche.

**Mapping progress (2026-11-10):** added two value-level V1 infrastructure
mapping cases for nested identifiers/names and preserving null versus empty
collections. The infrastructure profile test class passed (3 tests including
configuration validity). This supplements rather than replaces other resource
mapping contracts; additional mapping cases are recorded below.

**Remaining mapping and logging progress (2026-11-10):** added funding-call
nested foundation/category and null-versus-empty assertions; funding-decision
null relationship/collection preservation and year-to-date conversion; and
dataset V1 FairDataUrl/null collection versus the smaller V2 response contract.
Added three Logging formatter tests for Interface/Indexer message shape,
Wood-metadata routing, and exception JSON. All focused profile/formatter tests
passed. Logging tests are in Indexer.Tests, which already references the shared
Logging project. Exporter coverage remains explicitly excluded.

**Service-free validation before FundingDecision HTTP coverage (2026-11-10):**
after the mapping, Virta, dataset-version, and Indexer failure-path additions,
the solution lane passed with Repository.Tests 28, Interface.Tests 406, and
Indexer.Tests 51: 485 passed, 0 failed, 0 skipped. No production-source changes
were made.

**Additional API and mapping coverage:** the V2 ResearchDataset mapping test now
checks all mapped names, the generated `ResearchfiUrl`, and the exact six-property
V2 shape. A populated Virta HTTP contract test seeds synthetic rows in an isolated
EF InMemory database and verifies selected serialized fields from all four routes,
including the `Julkaisutyyppi` to `RistiriitainenTieto` mapping. The focused V2
mapping tests passed (4), and the populated Virta test passed (1). These additions
do not verify Virta SQL translation or complete response contracts for every API
resource; Phase 4 remains in progress.

The research-dataset repository test now covers below/equal/above maximum version
boundaries and an empty version set (including normalization to null). Its focused
suite passed (10 tests), followed by a newer service-free solution run (482 passed).

**FundingDecision HTTP contract progress (2026-11-10):** added
[FundingDecisionHttpContractTest](../aspnetcore/test/Interface.Tests/FundingDecisionHttpContractTest.cs)
for all search filters, pagination binding/headers, representative nested response
JSON, and malformed year/page parameters rejected before service invocation. The
focused contract tests passed (4). The subsequent service-free solution run passed
with Repository.Tests 28, Indexer.Tests 51, and Interface.Tests 410: 489 passed,
0 failed, 0 skipped. Phase 4 remains in progress.

Publication aggregation coverage now verifies co-publication keyword, publication
type category, field-of-science, and field-of-art merges, with duplicate related
values/codes retained only once. A characterization case additionally shows that
two distinct `Name` instances with the same `NameId` both survive contribution
aggregation; this key-selection issue is tracked as P4-PUB-CONTRIB rather than
fixed without a domain decision. The focused PublicationIndexRepositoryTest suite
passed (9 tests), and the service-free solution suite passed with 483 tests.

Indexer orchestration now also covers a chunk-write failure: the error is logged,
the partially built index is not activated, and processing continues to the next
configured type. The focused suite also verifies index-creation failure prevents
chunk retrieval or activation while processing continues. A full (non-chunked)
index-write failure is now also verified to be logged without blocking a later
configured type. Organization and funding-call cache retrieval failures are
verified to propagate before target indexing begins. The focused Indexer suite
also verifies index-name resolution failure prevents index creation and chunk
retrieval while a later configured type continues. The focused suite passed (14
tests), followed by the service-free solution suite (481 passed).

Additional Indexer coverage verifies that full and chunked indexing submit the
repository's bulk and per-entity in-memory transformation results rather than
the raw source objects. The focused Indexer suite passed (16 tests); the latest
service-free solution run passed with 485 tests.

Non-chunked repository retrieval failure is now also characterized: the exception
is logged and the next configured type is processed. This is test-only coverage
of current behavior, not an endorsement of swallowing the failed type's error.
The focused Indexer suite passed (17 tests). The service-free solution suite then
passed with Repository.Tests 28, Indexer.Tests 52, and Interface.Tests 410:
490 passed, 0 failed, 0 skipped.

After adding successful funder update/delete persistence coverage, the focused
ApiRoutePolicyTest suite passed (98), and the service-free solution suite passed
with Repository.Tests 28, Indexer.Tests 52, and Interface.Tests 412: 492 passed,
0 failed, 0 skipped.

Indexer coverage also verifies that a non-chunked bulk transformation returning
no documents causes no write or index rotation. The focused Indexer suite passed
(18 tests); the service-free solution suite passed with Repository.Tests 28,
Indexer.Tests 53, and Interface.Tests 412: 493 passed, 0 failed, 0 skipped.

Chunk retrieval failure after a successfully written 15,000-document first batch
is now covered. The test verifies all first-batch documents are submitted once,
the partially built index is not switched, the error is logged, and later
configured types continue. The focused Indexer suite passed (19 tests). The
service-free solution suite passed with Repository.Tests 28, Indexer.Tests 54,
and Interface.Tests 412: 494 passed, 0 failed, 0 skipped.

**Phase 4 acceptance decision (2026-11-10):** the user accepted representative
populated HTTP response contracts as sufficient for this phase; additional
resource-specific populated JSON contracts beyond funding-call, funding-decision,
and Virta remain deferred. The findings P3-ORG, P3-TIME, P4-PUB-CONTRIB,
P4-SEARCH, P4-COUNT, P4-ROTATE, P4-PROFILE, and P4-LINK are accepted as
time-bounded coverage exceptions, owned by Repository maintainers and due for
review by 2026-12-10. They remain documented in the
[defect follow-up plan](test-coverage-defect-follow-up-plan.md); fixes are not
authorized by this acceptance. The latest service-free suite passes (494, 0
failed, 0 skipped). **Phase 4 complete (2026-11-10):** the user reports that
GitHub CI passed for the current changes, including the Elasticsearch and SQL
Server integration lanes on supported Linux x86-64. The exact workflow run was
not independently retrieved in this session. Phase 5 CI coverage gates remain
unstarted and are not part of this acceptance.

| Priority | Production surface | Required scenarios |
| --- | --- | --- |
| P1 | [Query generators](../aspnetcore/src/ElasticService/ElasticSearchQueryGenerators) | For all seven generators: defaults, each supported filter, exact vs partial matching, nested relationships, combined filters, date/year boundaries, sort and pagination. Inspect serialized DSL with the production serializer; verify representative semantics against Elasticsearch. |
| P1 | [ElasticSearchService](../aspnetcore/src/ElasticService/ElasticSearchService.cs) | Empty/single/multiple hits, totals, missing single document, search-after initial/next/final page, count request filter parity, and search/count failures. Characterize existing count-error suppression explicitly; agree a separate fix if it violates error-reporting requirements. |
| P1 | [ElasticSearchIndexService](../aspnetcore/src/ElasticService/ElasticSearchIndexService.cs) | Existing rotation cases plus invalid index name, create failure, bulk item/transport errors, and empty/1/1499/1500/1501-document boundaries. Assert documents indexed once, request order, and that bulk failure prevents alias switch/old-index deletion. Exercise health/alias/delete failures to identify unsafe transitions; do not make silent failures the intended specification. |
| P1 | [API controllers](../aspnetcore/src/Interface/Controllers) and [ResponseHelper](../aspnetcore/src/Interface/Controllers/ResponseHelper.cs) | Data-driven route/method/policy matrix: 401 anonymous, 403 wrong role, success with correct role, query binding, invalid requests, exact public JSON shape, pagination headers/links with filters preserved, empty results, missing publication, and export token traversal without duplicates/omissions. Cover hidden routes without exposing them. Include funder-write and Virta paths with isolated dependencies and policy-specific assertions. |
| P1 | [ProfileBackendClient](../aspnetcore/src/Interface/Services/ProfileBackendClient.cs) and [ProfileController](../aspnetcore/src/Interface/Controllers/V1/ProfileController.cs) | Success payload, actual POST path/body, configured token and optional client-ID forwarding, cancellation, error status/body/content-type preservation, malformed/empty success payloads, and network errors. Test typed-client DI and RequestAborted forwarding. Never log tokens or real profile data. |
| P1 | [Repositories](../aspnetcore/src/Repositories) and [mapping profiles](../aspnetcore/src/Repositories/Maps) | Null/sentinel IDs, missing/duplicate relationships, organization-cache hits/misses, funding-call enrichment, dataset latest-version behavior, and publication aggregation. Add organization/infrastructure/infrastructure-service cases where coverage is absent; exercise both bulk and single-entity post-processing because the indexer uses both paths. |
| P1 | [Indexer](../aspnetcore/src/Indexer/Indexer.cs) | Cache population, missing repository logging, small/full/chunked inputs, 14999/15000/15001-row boundaries, final empty chunk, failure before activation, and no lost/duplicated documents. Verify observable logging/continuation behavior; raise behavior changes separately. |
| P2 | [DatabasePreflightCheck](../aspnetcore/src/Indexer/DatabasePreflightCheck.cs) | Exact 80% threshold and below/above it, zero/empty tables, sentinel-row filtering, and failure logging. Agree invalid/impossible-count expectations rather than inventing input behavior. |
| P2 | [Interface mappings](../aspnetcore/src/Interface/Maps) and [middleware](../aspnetcore/src/Interface/Middleware) | Supplement configuration-validity tests with actual null/collection/sentinel mapping examples, V1/V2 contract differences, correlation-ID generation/propagation, exception response sanitization, and logging. |
| P2 | [Logging](../aspnetcore/src/Logging) | Cover formatter output shape and exception fields if warranted by the baseline. Exporter coverage is excluded because the project is planned for removal from the solution. |

**Acceptance:** each P1 row has positive, negative, and relevant boundary tests,
not merely configuration/constructor execution, or a finding-linked exception
where a production defect prevents the desired assertion. Every discovered
defect is recorded in the [follow-up plan](test-coverage-defect-follow-up-plan.md)
with reproducing evidence and an explicit test disposition. Production fixes and
testability refactors are out of scope; only confirmed unused code may be removed.

### Phase 5 - Make coverage sustainable (P1, 1-2 days)

**Dependencies:** phase 1 reports; phase 2 stable fast lane. Promote integration jobs only after their fixtures are repeatable.

- [ ] Split CI into fast, Elasticsearch, and SQL Server test lanes with non-overlapping filters. Verify each lane discovers a nonzero expected set and that their union covers every intended test.
- [ ] Upload per-lane TRX, service logs on failure, and coverage reports even when tests fail. A final report job must not hide a failed prerequisite job.
- [ ] Set the measured per-assembly line/branch baseline as the initial no-regression floor; review denominator changes explicitly. Include assembly-level output so a large mapping suite cannot mask untested index/search logic.
- [ ] Proposed steady-state target: at least 80% line and 70% branch coverage on changed handwritten logic, with reviewed exceptions for tiny/uninstrumentable changes. Establish collection/merge/diff tooling and an observation period before enforcing these targets; do not invent an achievable repository-wide percentage before phase 1.
- [x] Observe three consecutive successful CI runs of each integration lane, with separate parallel-isolation evidence, before making it required (revised Phase 3 gate, satisfied by maintainer-reported CI results and local isolation tests). Continue monitoring subsequent PRs; keep failures visible and owned, never silently skipped.
- [ ] Give every remaining quarantine/skip a reason, owner, tracking reference, and review date; disallow new unconditional skips or commented-out tests without that disposition.
- [ ] Add test documentation under the existing test tree covering tier filters, coverage commands, fixture provisioning, supported service versions, cleanup, troubleshooting, and platform limitations.

**Acceptance:** PR checks reliably execute the intended lanes and enforce the approved coverage floor; reports remain available on failure; no dependency on production services or developer-specific secrets.

## 4. Validation commands for implementation

Run from `aspnetcore/`. The project currently uses xUnit/VSTest; verify platform selection before changing tooling. Build first; the `--no-build` examples intentionally use current binaries. Run each project separately so TRX and coverage output cannot overwrite one another.

```text
dotnet build PublicApi.sln --no-restore
dotnet test test/Interface.Tests/Interface.Tests.csproj --no-build --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger "trx;LogFileName=Interface.Tests.trx" --results-directory TestResults/Interface.Tests
dotnet test test/Indexer.Tests/Indexer.Tests.csproj --no-build --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger "trx;LogFileName=Indexer.Tests.trx" --results-directory TestResults/Indexer.Tests
dotnet test test/Repository.Tests/Repository.Tests.csproj --no-build --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger "trx;LogFileName=Repository.Tests.trx" --results-directory TestResults/Repository.Tests
python3 test/coverage_report.py TestResults --source-root src --output TestResults/coverage-summary.md
```

Save each project's `--list-tests` output as `discovery.txt` in that project's results directory before generating the report. Use `py -3` instead of `python3` on Windows. The initial baseline includes existing skips. After traits and the shared settings exist:

```text
dotnet test PublicApi.sln --no-build --filter "Category!=ElasticsearchIntegration&Category!=SqlServerIntegration" --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults/fast
dotnet test PublicApi.sln --no-build --filter "Category=ElasticsearchIntegration" --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults/elasticsearch
dotnet test PublicApi.sln --no-build --filter "Category=SqlServerIntegration" --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults/sqlserver
```

These are planned commands, not evidence of a passing suite. Start with affected classes/projects when implementing; run the whole applicable lane before merging. Provision services before their selected command and verify readiness explicitly. Validate exact identities, payloads, headers, document counts, and boundaries, not only HTTP success or aggregate percentages.

The Elasticsearch lane requires Docker and uses Testcontainers to start Elasticsearch 8.19.22 on an automatically assigned host port. Its fixture verifies the server version, creates uniquely named indices with production AutoMap configuration, refreshes them before searching, deletes only those indices, and disposes the container. CI runs this lane separately from the service-free fast lane.

The SQL Server lane also uses Testcontainers and an assigned port. It creates a unique database in its own SQL Server 2022 CU27 container and applies the embedded funding-call and funding-decision schema/seed scripts. See the [test README](../aspnetcore/test/README.md) for precise schema scope, platform/tooling limitations, diagnostics, and commands. An unfiltered test run includes both service lanes and requires Docker.

## 5. Delivery, risks, and decisions

**Suggested sequence:** PR 1 baseline/reporting -> PR 2 async/auth/host repairs -> PRs 3a/3b Elasticsearch and SQL fixture/recovery work -> focused P1 coverage PRs -> CI enforcement. Unit/API coverage expansion can run alongside service provisioning after host repair.

**Effort:** approximately 10-16 developer-days for phases 1-4 plus 1-2 days for CI policy/documentation; SQL schema provisioning is the largest uncertainty. Re-estimate after phase 1. The ten-run observation period is elapsed CI time, not included developer effort. Exporter coverage is excluded; any P2 logging-formatter coverage should be sized separately from the baseline.

| Risk / open decision | Mitigation / owner role |
| --- | --- |
| Unknown coverage and current test health | Test maintainer owns phase 1; distinguish measurements from proposed targets. |
| Elasticsearch version/mapping drift | API/indexer maintainer confirms supported server version and production mapping parity. |
| SQL schema is scaffolded and externally managed | Data maintainer approves minimal versioned test-schema provisioning; never connect integration tests to the warehouse. |
| SQL Server container support differs across Mac/Windows/ARM | Prove provisioning in Linux CI; document a supported isolated development option rather than assuming every machine can run the image. Keep the fast lane platform-independent. |
| Index switching deletes indices; write tests mutate data | Enforce unique test-owned names and database identity checks before setup/cleanup. Delete only resources allocated by that fixture, never wildcard/shared indices or databases. |
| Environment variables, static logging, caches and auth leak state | Host-local configuration and fixture ownership; serialize only the specific unavoidable shared-resource tests. |
| Funding-call POST appears supported but does nothing | API owner decides intent; do not restore writes or remove authorization as a coverage task. |
| Reinstating old expectations exposes intentional changes | Compare current contracts and production mapping/post-processing before restoring assertions; document retirement rather than force obsolete behavior. |
| Coverage gaming or flaky integration checks | Branch and per-assembly reporting, negative controls, strict setup failure, repeated clean runs, and explicit skip ownership. |

There is no production deployment in this workstream: changes should be tests, test-only infrastructure/configuration, CI, and directly related documentation. Production seams or bug fixes require separate review and deployment validation; never loosen production auth or Swagger visibility to help tests.

## 6. Completion checklist

- [ ] Baseline and final test/coverage reports are persisted as CI artifacts, with per-assembly line/branch deltas and suite durations.
- [ ] All remaining skipped and commented methods have explicit final dispositions; POST is decided or remains a visibly owned exception.
- [ ] Async tests observe completion; no always-true predicates or task-only success assertions remain in repaired tests.
- [ ] Fast tests are isolated; real Elasticsearch/SQL lanes use synthetic data, fail loudly on missing infrastructure, and clean up only their own resources.
- [ ] All P1 scenarios in section 3 are covered, with no unauthorized endpoint exposure or accidental contract changes.
- [ ] CI reporting/gates and cross-platform local instructions are verified.
- [ ] Record any deferred P2 work explicitly. Keep this plan at the top level while tasks remain; only when complete, summarize outcomes/files and move it to `plans/done/`.
