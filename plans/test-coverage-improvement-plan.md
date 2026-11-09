Status: Draft
2026-11-09

# Test coverage improvement plan

## Goal and scope

Make existing tests trustworthy, recover useful disabled scenarios, and add risk-focused coverage across the SQL Server -> repositories -> indexer -> Elasticsearch -> HTTP API pipeline.

This is a plan only: do not enable tests, change application behavior, or introduce infrastructure as part of this document. Preserve authorization policies, intentionally hidden endpoints, V2 documentation visibility, and current public contracts. Any newly exposed application defect needs a focused regression test and a separately reviewed behavior change.

## 1. Findings from source inspection

No tests or coverage collection were executed for this plan. Passing/discovered test counts, runtime, and line/branch percentages must be measured in phase 1; source attribute counts are not runtime test-case counts.

### Existing foundation

- [PublicApi.sln](../aspnetcore/PublicApi.sln) includes all three test projects: [Interface.Tests](../aspnetcore/test/Interface.Tests/Interface.Tests.csproj), [Indexer.Tests](../aspnetcore/test/Indexer.Tests/Indexer.Tests.csproj), and [Repository.Tests](../aspnetcore/test/Repository.Tests/Repository.Tests.csproj).
- Tests use xUnit and FluentAssertions; Moq, WebApplicationFactory, an EF InMemory package, and an in-memory Elasticsearch transport are already available in Interface.Tests.
- Active tests cover several mapping profiles, repository in-memory transformations, URL generation, index-name configuration, middleware, Swagger V1, and three index-rotation success paths. Profile controller/client tests currently cover one backend-400 scenario each.
- [CI](../.github/workflows/dotnet.yml) restores, builds, and tests on PRs to `devel`/`main`, but does not collect coverage or upload test/coverage artifacts.
- All three test projects reference `coverlet.collector`, but use older, differing collector/Test SDK versions. First verify compatibility with .NET 10; do not make a blanket test-framework migration a prerequisite.

### Inactive test inventory and disposition

There are **15 explicitly skipped test methods**: ten GET theories, one POST fact, and four repository debugging facts. Three additional facts are inside block comments and therefore absent from discovery. Theory data can expand into multiple runtime cases.

| Location | Inactive methods | Problem / proposed disposition |
| --- | ---: | --- |
| [FundingCallSystemTest](../aspnetcore/test/Interface.Tests/FundingCallSystemTest.cs) | 2 GET theories, 1 POST fact | GETs assume pre-existing Elasticsearch data and use the old `v1/FundingCall` route. Rewrite against seeded data and `v1/funding-calls`. Do not simply enable POST: the service's persistence code is commented out. |
| [FundingDecisionSystemTest](../aspnetcore/test/Interface.Tests/FundingDecisionSystemTest.cs) | 2 GET theories | Old route, hard-coded real records, and deserialization into an internal service model. Use `v1/funding-decisions`, public API response models, and synthetic expected IDs. |
| [InfrastructureSystemTest](../aspnetcore/test/Interface.Tests/InfrastructureSystemTest.cs) | 2 GET theories | Old route and a name predicate that unconditionally returns `true`. Replace with meaningful positive/negative assertions against `v1/infrastructures`. |
| [OrganizationSystemTest](../aspnetcore/test/Interface.Tests/OrganizationSystemTest.cs) | 2 GET theories | Old route and live-data assumptions. Use seeded `v1/organizations` tests; keep the endpoint hidden from Swagger. |
| [ResearchDatasetSystemTest](../aspnetcore/test/Interface.Tests/ResearchDatasetSystemTest.cs) | 2 GET theories | Old route and live-data assumptions. Rewrite for `v1/research-datasets`; cover V2 separately without publishing V2 Swagger. |
| [FundingCallRepositorySystemTest](../aspnetcore/test/Interface.Tests/FundingCallRepositorySystemTest.cs) | 1 debugging fact | Placeholder record name, API-model repository type, and API-host coupling. Replace useful intent with an isolated repository test using the registered service-model type. |
| [FundingDecisionRepositorySystemTest](../aspnetcore/test/Interface.Tests/FundingDecisionRepositorySystemTest.cs) | 3 debugging facts | Real record names/acronyms and weak assertions; also imports TestHost's `Program`. Extract consortium/enrichment requirements into deterministic repository tests and remove obsolete debugging scaffolding. |
| [Indexer publication mapping tests](../aspnetcore/test/Indexer.Tests/Maps/PublicationProfileTest.cs) | 2 commented facts | Old co-/organization-publication expectations. Reconcile with current projection DTOs and repository post-processing, then restore scenarios at the appropriate layer rather than uncommenting outdated code. |
| [SwaggerSystemTest](../aspnetcore/test/Interface.Tests/SwaggerSystemTest.cs) | 1 commented V2 fact | [Swagger configuration](../aspnetcore/src/Interface/Configuration/SwaggerConfiguration.cs) deliberately does not register V2. Replace with a test that V2 remains undocumented; do not enable its old successful-document expectation. |

### Reliability problems to fix before growing test counts

1. [FundingCallControllerTest](../aspnetcore/test/Interface.Tests/FundingCallControllerTest.cs) does not await controller calls. Asserting a non-null `Task` and verifying a dependency call cannot prove successful completion or valid response headers.
2. Skipped GET tests use `async void` and resolve `ApiDbContext` even though API-host registration of that context/repositories is commented out in [Program](../aspnetcore/src/Interface/Program.cs). Search tests must not require SQL Server.
3. [TestWebApplicationFactory](../aspnetcore/test/Interface.Tests/TestWebApplicationFactory.cs) changes process-wide environment variables and points Elasticsearch at localhost; it does not replace outbound services. This risks configuration leakage between parallel tests and accidental network access.
4. [TestAuthHandler](../aspnetcore/test/Interface.Tests/TestAuthHandler.cs) always authenticates with every policy role. Existing happy-path tests cannot prove 401/403 enforcement.
5. [MockElasticConnection](../aspnetcore/test/Interface.Tests/TestHelpers/MockElasticConnection.cs) returns generic successful bodies for unhandled requests. That is useful for request inspection, but cannot prove real search matching, bulk-item success, or cluster health semantics.
6. [FundingCallService.PostFundCall](../aspnetcore/src/Interface/Services/V1/FundingCallService.cs) currently performs no persistence. Product intent must be decided before specifying a successful write test.

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

- [ ] Discover tests from the solution and record executed/passed/failed/skipped counts per project, with expanded theory counts distinguished from method counts.
- [ ] Capture current failures before changing tests. Separate environment/configuration failures from assertion failures and .NET 10 tooling compatibility failures.
- [ ] Collect Cobertura coverage using the existing collector. If collection/discovery fails due to package compatibility, make the smallest verified Test SDK/collector upgrade and rerun all three projects.
- [ ] Add a shared runsettings file and CI artifact upload for TRX and coverage, including results on failed runs. Add a repository-local report/merge step only if needed; no third-party code-upload service.
- [ ] Merge coverage from all test projects by covered source lines/branches, not by averaging percentages; shared production assemblies are exercised by multiple projects.
- [ ] Publish overall and per-production-assembly line/branch coverage. Exclude test assemblies, build outputs, and scaffolded DatabaseContext code. Keep handwritten repositories, query generators, middleware, and orchestration included. Document any other generated-code exclusions narrowly.
- [ ] Produce the ranked uncovered-method list, suite durations, and an explicit inactive-test disposition checklist based on section 1.

**Acceptance:** reproducible discovery and coverage reports for all three projects; no empty report accepted as success; runtime percentages and unresolved failures are recorded without claiming an unmeasured baseline.

### Phase 2 - Repair test reliability and host isolation (P0, 2-3 days)

**Dependencies:** phase 1 baseline.

- [ ] Rewrite the four funding-call pagination tests as `async Task`; configure meaningful search results, assign an HTTP context, await completion, and assert returned values and headers as well as arguments.
- [ ] Convert touched `async void` tests to `async Task`; dispose clients/scopes/responses appropriately.
- [ ] Replace process-wide test configuration mutation with host-local configuration. Explicitly replace search/profile clients and any database services used by a test. Keep test secrets synthetic and prevent real Elasticsearch, Keycloak, SQL Server, profile-backend, and logging-sink calls in the fast lane.
- [ ] Make test authentication selectable: anonymous, authenticated without required role, and authenticated with exactly the required role. Include client identity claims where relevant.
- [ ] Extend the existing NEST test transport with realistic, scenario-specific search/bulk/health responses. Fail explicitly on unexpected requests in new tests; preserve and then strengthen existing index-rotation assertions.
- [ ] Check Scrutor and typed HttpClient registration through the test host, not only manually constructed controllers.

**Acceptance:** fast tests pass without running external services or using developer secrets; a deliberately faulted async dependency fails its test; parallel test hosts do not leak configuration/auth state. All four pagination cases assert completed results.

### Phase 3 - Recover inactive scenarios safely (P1, 3-5 days)

**Dependencies:** phase 2; fixture/provisioning work can proceed independently for Elasticsearch and SQL Server.

#### Elasticsearch search recovery

- [ ] Provision the supported Elasticsearch version from deployment requirements; pin the test image, bound startup/readiness time, and surface service logs on failure.
- [ ] Seed minimal synthetic documents through production index mappings. Use unique test-owned index/alias names and configure the API host to use them; explicitly refresh before searching.
- [ ] Rewrite the ten skipped GET theories against the current routes/public response types. Remove unnecessary `ApiDbContext` resolution.
- [ ] Preserve useful name/funder/business-ID/category/date/type-of-funding cases, but reconcile old parameter names and boundary semantics with current query generators first.
- [ ] Include matching documents, near misses, and combined-filter counterexamples. Assert exact returned identifiers/counts plus public fields; `OnlyContain` alone must not miss silently omitted results.
- [ ] Replace the always-true infrastructure predicate with field-level assertions. Add a negative control proving a nonmatching infrastructure is excluded.
- [ ] Add publication search/detail and infrastructure-service scenarios missing from the inactive suite; cover V2 dataset behavior as an HTTP contract distinct from documentation visibility.

#### Repository and publication recovery

- [ ] Replace the four debugging facts with synthetic tests in Repository.Tests: funding-call retrieval intent, funding-decision consortium enrichment, and relevant post-processing. Retire placeholder/debug-only cases explicitly rather than preserving meaningless assertions.
- [ ] Build only the SQL schema surface needed for initial repository scenarios using an approved, versioned schema source. Scaffolded entities are not migrations: do not assume `EnsureCreated` can reproduce the warehouse or hand-edit generated entities.
- [ ] Run relational queries against an isolated SQL Server database; supply required reference/sentinel rows and relationships explicitly. Do not copy production records or credentials.
- [ ] Restore both publication scenarios across mapping and post-processing as appropriate: parent/child IDs, organization/co-publication flags, and contribution aggregation with missing/duplicate relationships.
- [ ] Replace commented V2 Swagger code with an assertion of the intended non-published behavior; retain exact V1 path checks and explicitly assert intentionally hidden paths remain absent.
- [ ] Decide whether funding-call POST is supported, disabled, or obsolete with the API owner. Until decided, keep its existing skip tracked as an explicit exception. If supported, plan persistence restoration separately and test writes only in a disposable database; never make no-op success the desired contract.

**Acceptance:** all 18 inactive methods have a recorded disposition (active replacement, intentional retirement, or owned/time-bounded exception). Ten GET-theory scenarios execute in the Elasticsearch lane with synthetic fixtures. A decision and follow-up are recorded for POST; no unexplained permanent skips or commented-out tests remain. Failed setup is not reported as a successful/empty integration run.

### Phase 4 - Expand high-risk branch and contract coverage (P1/P2, 4-6 days)

**Dependencies:** phase 2 for unit/API work; phase 3 fixtures for real-service cases. Start the highest-risk items before waiting for every recovery task.

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
| P2 | [Exporter](../aspnetcore/src/Exporter/Exporter.cs) and [Logging](../aspnetcore/src/Logging) | Use the baseline to size follow-up coverage: exporter empty/multipage input, file counts/content/names, missing directory setting and I/O/search failure; formatter output shape and exception fields. Use test-owned temporary directories with targeted cleanup. Add a project reference/new test project only when these tests require it. |

**Acceptance:** each P1 row has positive, negative, and relevant boundary tests, not merely configuration/constructor execution. Every discovered defect has a regression test and an explicit fix/decision; existing public behavior is not accidentally changed to make tests pass.

### Phase 5 - Make coverage sustainable (P1, 1-2 days)

**Dependencies:** phase 1 reports; phase 2 stable fast lane. Promote integration jobs only after their fixtures are repeatable.

- [ ] Split CI into fast, Elasticsearch, and SQL Server test lanes with non-overlapping filters. Verify each lane discovers a nonzero expected set and that their union covers every intended test.
- [ ] Upload per-lane TRX, service logs on failure, and coverage reports even when tests fail. A final report job must not hide a failed prerequisite job.
- [ ] Set the measured per-assembly line/branch baseline as the initial no-regression floor; review denominator changes explicitly. Include assembly-level output so a large mapping suite cannot mask untested index/search logic.
- [ ] Proposed steady-state target: at least 80% line and 70% branch coverage on changed handwritten logic, with reviewed exceptions for tiny/uninstrumentable changes. Establish collection/merge/diff tooling and an observation period before enforcing these targets; do not invent an achievable repository-wide percentage before phase 1.
- [ ] Run each new integration lane successfully at least ten consecutive times on a clean CI environment, including parallel-isolation checks, before making it required. During observation, keep failures visible and owned, never silently skipped.
- [ ] Give every remaining quarantine/skip a reason, owner, tracking reference, and review date; disallow new unconditional skips or commented-out tests without that disposition.
- [ ] Add test documentation under the existing test tree covering tier filters, coverage commands, fixture provisioning, supported service versions, cleanup, troubleshooting, and platform limitations.

**Acceptance:** PR checks reliably execute the intended lanes and enforce the approved coverage floor; reports remain available on failure; no dependency on production services or developer-specific secrets.

## 4. Validation commands for implementation

Run from `aspnetcore/`. The project currently uses xUnit/VSTest; verify platform selection before changing tooling. Build first; the `--no-build` examples intentionally use current binaries.

```text
dotnet build PublicApi.sln --no-restore
dotnet test PublicApi.sln --no-build --list-tests
dotnet test PublicApi.sln --no-build --logger trx --collect:"XPlat Code Coverage" --results-directory TestResults
```

The initial baseline includes existing skips. After traits and the shared settings exist:

```text
dotnet test PublicApi.sln --no-build --filter "Category!=ElasticsearchIntegration&Category!=SqlServerIntegration" --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults/fast
dotnet test PublicApi.sln --no-build --filter "Category=ElasticsearchIntegration" --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults/elasticsearch
dotnet test PublicApi.sln --no-build --filter "Category=SqlServerIntegration" --settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults/sqlserver
```

These are planned commands, not evidence of a passing suite. Start with affected classes/projects when implementing; run the whole applicable lane before merging. Provision services before their selected command and verify readiness explicitly. Validate exact identities, payloads, headers, document counts, and boundaries, not only HTTP success or aggregate percentages.

## 5. Delivery, risks, and decisions

**Suggested sequence:** PR 1 baseline/reporting -> PR 2 async/auth/host repairs -> PRs 3a/3b Elasticsearch and SQL fixture/recovery work -> focused P1 coverage PRs -> CI enforcement. Unit/API coverage expansion can run alongside service provisioning after host repair.

**Effort:** approximately 10-16 developer-days for phases 1-4 plus 1-2 days for CI policy/documentation; SQL schema provisioning is the largest uncertainty. Re-estimate after phase 1. The ten-run observation period is elapsed CI time, not included developer effort. P2 exporter/formatter work should be sized separately from the baseline.

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
- [ ] All 15 skipped and three commented methods have explicit final dispositions; POST is decided or remains a visibly owned exception.
- [ ] Async tests observe completion; no always-true predicates or task-only success assertions remain in repaired tests.
- [ ] Fast tests are isolated; real Elasticsearch/SQL lanes use synthetic data, fail loudly on missing infrastructure, and clean up only their own resources.
- [ ] All P1 scenarios in section 3 are covered, with no unauthorized endpoint exposure or accidental contract changes.
- [ ] CI reporting/gates and cross-platform local instructions are verified.
- [ ] Record any deferred P2 work explicitly. Keep this plan at the top level while tasks remain; only when complete, summarize outcomes/files and move it to `plans/done/`.
