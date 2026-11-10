# Running tests

Run commands from `aspnetcore/`. Tests use xUnit and VSTest.

## Service-free tests

```shell
dotnet test PublicApi.sln --filter "Category!=ElasticsearchIntegration&Category!=SqlServerIntegration"
```

This lane does not require Docker, SQL Server, Elasticsearch, or developer credentials.

Funding calls support GET only. Service-free tests verify POST returns HTTP 405
for anonymous, authenticated, and role-bearing callers (including the legacy write
role), and Swagger lists only GET for `/v1/funding-calls`. The obsolete no-op POST
and skipped persistence test have been retired.

### Query-generator contracts

```shell
dotnet test test/Interface.Tests/Interface.Tests.csproj --filter "FullyQualifiedName~QueryGeneratorContractTest"
```

[QueryGeneratorContractTest](Interface.Tests/QueryGenerators/QueryGeneratorContractTest.cs)
resolves all seven generators and the NEST serializer through production
registration, but only serializes requests: it makes no Elasticsearch calls.
It checks configured indices, pagination and sort order, optional long export
cursors, search/count/export filter parity, individual and combined filter
clauses, nested paths and author correlation, date/year boundaries, visibility,
and implemented single-document lookups.

Serialized DSL checks complement, rather than replace, the container lane's
matching/nonmatching document assertions. Organization name-search semantics
remain deferred under P3-ORG. Production defects found during coverage work are
recorded in the [separate follow-up plan](../../plans/test-coverage-defect-follow-up-plan.md);
they are not fixed as part of the coverage plan.

### Search-service responses and failures

```shell
dotnet test test/Interface.Tests/Interface.Tests.csproj --filter "FullyQualifiedName~ElasticSearchServiceTest"
```

[ElasticSearchServiceTest](Interface.Tests/Services/ElasticSearchServiceTest.cs)
uses real NEST serialization and response parsing through a strict scripted
in-memory transport; no Docker or network access is needed. It verifies
documents/totals, single lookups, full/partial/empty search-after pages, long
cursors, counts above 10,000, query parity, and request order.

HTTP 400/503 cases exercise both NEST's default non-throwing behavior and
explicit throwing settings, with retries disabled for deterministic responses.
Failure-characterization cases document P4-SEARCH and P4-COUNT in the
[follow-up plan](../../plans/test-coverage-defect-follow-up-plan.md).
They do not define successful empty results or silent count suppression as
desired behavior, and do not change production error handling.

### Index-service batching and lifecycle failures

```shell
dotnet test test/Interface.Tests/Interface.Tests.csproj --filter "FullyQualifiedName~ElasticSearchIndex"
```

[ElasticSearchIndexBoundaryTest](Interface.Tests/Services/ElasticSearchIndexBoundaryTest.cs)
adds full/chunk indexing cases for 0/1/1499/1500/1501 documents. It checks actual
NDJSON payloads, uniqueness/order, batch sizes, alias actions, readiness parameters,
creation failure, bulk item/HTTP failures, logging, and stopping before activation.
The in-memory transport returns one successful bulk item per submitted document
and uses real NEST response parsing, without Docker or external calls.

Lifecycle failure characterization is tracked as P4-ROTATE in the
[follow-up plan](../../plans/test-coverage-defect-follow-up-plan.md).
Those cases document existing unsafe continuation, not desired rotation behavior.
No production changes are made; absent-index deletion remains an allowed control.

### Profile contracts and authorization

```shell
dotnet test test/Interface.Tests/Interface.Tests.csproj --filter "FullyQualifiedName~ProfileBackendContractTest|FullyQualifiedName~ProfileEndpointContractTest|FullyQualifiedName~ProfileControllerTest"
```

These tests isolate outbound HTTP with message handlers, while exercising the
real backend client and the API's typed-client registration. They cover request
path/body, the synthetic configured token, optional client identity, success/error
responses, cancellation and network errors, controller RequestAborted forwarding,
role boundaries, and invalid request bodies. No real profile data or credentials
are used. JSON-null success behavior is characterized under P4-PROFILE in the
[follow-up plan](../../plans/test-coverage-defect-follow-up-plan.md), not fixed or
declared the intended response contract.

### API policy and pagination contracts

```shell
dotnet test test/Interface.Tests/Interface.Tests.csproj --filter "FullyQualifiedName~ApiRoutePolicyTest|FullyQualifiedName~FundingCallHttpContractTest|FullyQualifiedName~FundingDecisionHttpContractTest|FullyQualifiedName~ResponseHelperContractTest"
```

The route matrix covers search/export/detail, hidden organization/V2, Virta, and
funder read/write policies with anonymous, no-role, wrong-role, and correct-role
callers. Search services are isolated fakes; Virta and import databases are
uniquely named EF InMemory stores. These cases do not verify SQL translation.
The profile route has its own policy matrix described above.

`VirtaHttpContractTest` seeds synthetic rows in an isolated EF InMemory store and
checks populated JSON from all four Virta routes, including the
`Julkaisutyyppi`-to-`RistiriitainenTieto` public-field mapping. This verifies
controller mapping and response shape, not SQL translation.

Funder route tests verify POST persistence, successful PUT/DELETE behavior,
client isolation, and not-found cases using an isolated EF InMemory store.
Authorization is checked independently for each read/write route.

Funding-call HTTP cases verify query binding, invalid queries, public JSON,
pagination normalization, and export-link traversal without duplicates/omissions.
Helper cases assert exact link relations, preserved filters, and cursor headers.
The page-zero link emitted for empty results is characterized as P4-LINK in the
[follow-up plan](../../plans/test-coverage-defect-follow-up-plan.md).

`FundingDecisionHttpContractTest` verifies all search filters and pagination bind
to the service call, representative nested response JSON and pagination headers,
and 400 responses for malformed year/page parameters before service invocation.

## Container integration tests

Start Docker Desktop (or a compatible Docker engine), then run the relevant lane:

```shell
dotnet test test/Interface.Tests/Interface.Tests.csproj --filter "Category=ElasticsearchIntegration"
dotnet test test/Repository.Tests/Repository.Tests.csproj --filter "Category=SqlServerIntegration"
```

An unfiltered `dotnet test` includes both integration lanes and therefore requires Docker.
The first run downloads images; subsequent runs can use the local image cache.
Setup failures fail the tests rather than silently skipping them.

| Lane | Container image | Provisioning and cleanup |
| --- | --- | --- |
| Elasticsearch | `docker.elastic.co/elasticsearch/elasticsearch:8.19.22` | Shared collection fixture starts one container, uses production AutoMap, seeds unique indices, refreshes them, and removes the indices and container. |
| SQL Server | `mcr.microsoft.com/mssql/server:2022-CU27-ubuntu-22.04` | Shared collection fixture starts one container with a unique database and generated test-only password, applies the embedded schema/seed scripts, and removes the container and its database. |

Both use automatically assigned host ports. No local Elasticsearch/SQL Server instance,
fixed port, connection-string secret, or `ELASTICSEARCH_TEST_URL` is used. Each test
process owns a separate container. SQL tests create a fresh `ApiDbContext` per test.
Read tests share seed data; timestamp characterization tests modify a row within a
rolled-back transaction. The collection serializes these tests within each process.
Future write tests must use equivalent isolation or allocate their own state.

SQL Server's Linux image is supported on x86-64. ARM development machines may run it
through Docker's x86-64 emulation, but that is not an officially supported SQL Server
platform. Linux x86-64 CI is the reference environment. On unsupported machines, run
the service-free lane locally and the SQL lane in Linux x86-64 CI; do not point the
fixture at a warehouse or shared database.

### Elasticsearch API coverage

The fixture provisions all seven search indices with production AutoMap settings.
It verifies the API host's index configuration and exact seeded document counts
before issuing HTTP requests. Every indexed document is synthetic.

[PublicationAndInfrastructureServiceSystemTest](Interface.Tests/PublicationAndInfrastructureServiceSystemTest.cs)
adds publication search/detail and infrastructure-service scenarios:

- Publications: combined filters, nested author-name correlation, exact identifiers,
  organization-part/co-publication visibility, stable year/tie-break ordering,
  pagination totals, public response mapping, relationship detail, and missing IDs.
  The main publication's Elasticsearch document ID deliberately differs from its
  public ID to verify detail lookup queries the public identifier.
- Infrastructure services: combined nested name/description/infrastructure/organization
  filters, exact local/URN/other identifiers, inclusive end-year boundaries, start/end
  year filters, export ordering, and filtered pagination totals.

Positive queries are paired with seeded near misses and negative controls. The
infrastructure-service detail route is not enabled in production and is not restored
by these tests. Exhaustive query-filter and authorization matrices remain Phase 4 work.

### SQL schema scope

[FundingCallSchema.sql](Repository.Tests/SqlServer/FundingCallSchema.sql),
[FundingDecisionSchema.sql](Repository.Tests/SqlServer/FundingDecisionSchema.sql), and
[PreflightAndInfrastructureSchema.sql](Repository.Tests/SqlServer/PreflightAndInfrastructureSchema.sql)
are versioned, test-only subsets of the approved scaffolded
[ApiDbContext](../src/DatabaseContext/ApiDbContext.cs) mappings, applied in that order.
They include the columns, keys, and relationships used by the funding-call and
funding-decision repository queries, preflight checks, and infrastructure and
infrastructure-service repository queries, not the entire warehouse or all its
constraints.
Required sentinels, multilingual links, organizations, categories, funding-group
contributors, consortium memberships, keywords, topics, and excluded counterexamples
are synthetic.

The fixture does not use `EnsureCreated`, run migrations, or modify generated entities.
When the scaffold or query changes, review the script against the mapped column
names/types and relationship keys, update the minimal surface as needed, and rerun
the relational tests. Funding-decision tests cover EU eligibility through a consortium
organization's PIC type (not merely a PIC-bearing funder or unrelated organization),
domestic/unknown sources, duplicate PICs, mixed consortiums, self/parent contributors,
programme ancestors, dates, related collections, enrichment, and source-row paging.
Publication relational coverage is not yet provided by this schema.

`PreflightAndInfrastructureSchema.sql` adds the columns queried by preflight and
infrastructure/infrastructure-service repository paths. Its sentinel and positive
rows are synthetic.
SQL-backed preflight tests execute the complete `IsGood` query and assert the 80%
reference boundary, sentinel exclusion, empty-table handling, and error/status logging.
Infrastructure-service enrichment tests use transaction-isolated infrastructure
PIDs, participant facts, descriptive items, and ESFRI references. Both sets are in the
SQL Server integration lane and require Docker.

The timestamp characterization tests record an existing production behavior: null
`created` or `modified` values in a funding decision cause SQL error 242 during the
projection from nullable warehouse timestamps to non-nullable model timestamps.
This is not considered successful data retrieval. The maintainer accepted retaining
this behavior for Phase 3 (P3-TIME); repository maintainers must revisit it before
Phase 4 acceptance and agree a timestamp contract before a separate production fix.
Other scenarios seed explicit valid timestamps.

### Repository post-processing

```shell
dotnet test test/Repository.Tests/Repository.Tests.csproj --filter "FullyQualifiedName~ResearchDatasetIndexRepositoryTest|FullyQualifiedName~InfrastructureIndexRepositoryTest|FullyQualifiedName~InfrastructureServiceIndexRepositoryTest"
```

These service-free cases exercise dataset contributor organization precedence,
sentinel-organization cleanup, and infrastructure-service PID normalization and
Research.fi link generation through both single-entity and bulk post-processing.
Infrastructure post-processing also checks organization-cache hits/misses and
identifier mapping. These service-free cases do not execute SQL projections or
database-backed infrastructure-service relationship enrichment.

`PublicationIndexRepositoryTest` covers publication post-processing and
co-publication contribution/data aggregation. A characterization case shows that
separate `Name` instances with the same `NameId` are retained as separate
contributions; this key-selection question is tracked as P4-PUB-CONTRIB in the
defect follow-up plan.

SQL-backed infrastructure repository and enrichment paths are covered separately:

```shell
dotnet test test/Repository.Tests/Repository.Tests.csproj --filter "FullyQualifiedName~InfrastructureServiceSqlServerTest"
dotnet test test/Repository.Tests/Repository.Tests.csproj --filter "FullyQualifiedName~InfrastructureSqlServerTest"
```

`InfrastructureServiceSqlServerTest` verifies infrastructure PIDs, responsible and
participating organizations (including duplicate and unrelated contribution rows),
infrastructure names, ESFRI values, and generated Research.fi links through
`PerformInMemoryOperation`. It uses the minimal test-only infrastructure query schema
and rolls back all per-test inserts.

`InfrastructureSqlServerTest` executes the real `InfrastructureIndexRepository.GetAll`
projection and post-processing. It verifies start/end dates, descriptive names,
organization relationships and cache-enriched identifiers, URN handling, and
exclusion of sentinel and `infra_old` rows. Both tests require Docker and use
transaction-isolated synthetic data.

### Indexer orchestration boundaries

```shell
dotnet test test/Indexer.Tests/Indexer.Tests.csproj --filter "FullyQualifiedName~IndexerTest"
```

These service-free tests exercise organization/funding-call cache population,
missing-repository logging, small empty/nonempty indexing, publication chunking
at 14,999/15,000/15,001 documents, final empty-chunk requests, exact document
uniqueness/order, and indexing of both bulk and per-entity repository
transformation results. A bulk transformation that filters out every document
does not issue an index write or rotate an index.
Repository and index-service failures verify logging and activation behavior.
A failed chunk write prevents alias switching while later configured
types are still processed. A failed index creation prevents chunk retrieval and
activation, while later configured types still run. They use fake repositories
and an index service; a failed non-chunked write is also logged without
preventing subsequent index types from running. Failures while populating either
prerequisite cache propagate before any index writes begin. Failed index-name
resolution likewise prevents creation and chunk retrieval while later types
continue. A non-chunked repository retrieval failure is logged and likewise does
not prevent subsequent types from running. Retrieval failure after a successful
chunk write verifies that the partial index is not activated and subsequent
types still run. These tests do not exercise SQL query translation or
Elasticsearch indexing.

`DatabasePreflightCheckTest` verifies the 80% publication-reference threshold,
including its exact boundary and zero-count helper result. The latter is only a
characterization of the threshold helper; the SQL Server integration tests execute
the full database preflight query and verify empty-table and logging behavior.
Run those cases with
`dotnet test test/Repository.Tests/Repository.Tests.csproj --filter "FullyQualifiedName~DatabasePreflightCheckSqlServerTest"`.

### Middleware behavior

The middleware tests run correlation-ID handling through an in-memory ASP.NET
Core test server. They verify generated and caller-supplied response headers,
downstream context propagation, sanitized JSON error responses, the unavailable-ID
fallback, and matching correlation IDs across the combined correlation/error
middleware pipeline. They use synthetic exceptions only and do not test production
logging sinks.

The V1 infrastructure mapping tests cover nested identifiers and names, and
preserve the distinction between null and empty collections in the public model.
They supplement the existing AutoMapper configuration check; they do not certify
all infrastructure or infrastructure-service API fields.

Funding-call mapping tests verify nested foundation/category values and preserve
null-versus-empty collections. Funding-decision tests cover nullable nested
values and the inclusive January 1 conversion for `FundingStartYearFrom`.
Research-dataset tests cover V1's generated FairDataUrl and null collection
behavior, alongside V2's intentionally smaller response contract. These are
targeted mapping contracts, not exhaustive field-by-field snapshots.

The shared Logging project formatter tests live in Indexer.Tests and verify JSON
shape, exception serialization, and application-specific Wood metadata routing.
No exporter test project or exporter reference is added; Exporter is excluded
because it is planned for removal from the solution.

### Troubleshooting and diagnostics

- Check `docker info` if the Docker endpoint cannot be found. Testcontainers supports
  Docker endpoint configuration through its standard environment/settings mechanisms.
- Allow enough Docker memory for the selected services and time for the initial pull.
- SQL Server startup has a five-minute cancellation limit; its output is forwarded to
  the test runner for setup-failure diagnostics.
- Fixtures dispose their containers, and Testcontainers' resource reaper handles
  abandoned test processes. Do not remove unrelated developer containers to troubleshoot.
- Organization-search integration tests remain explicitly skipped under the
  maintainer-approved Phase 3 exception P3-ORG for a production field-name mismatch.
  Repository maintainers must revisit it before Phase 4 acceptance; this is not a
  container setup failure or evidence that organization search works.

## Coverage and CI

Add these options to a filtered test command to collect coverage and test results:

```text
--settings test/coverage.runsettings --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults/<lane>
```

CI runs the service-free lane and separate Elasticsearch/SQL Server jobs. Integration
jobs upload TRX and coverage artifacts even on test failure. The agreed stability
gate is three consecutive successful CI runs of both integration lanes, with
separate parallel-isolation evidence, before making the checks required. This
replaces the original ten-run proposal.

On 2026-11-09, the maintainer reported three successful CI runs after pushing the
changes and opening a PR. Together with the earlier local parallel-isolation tests,
this completes Phase 3 with accepted exceptions. Sequential CI reruns do not
themselves test parallel isolation. Continue monitoring subsequent PRs for
intermittent failures; required branch-check settings are managed separately.
See the [Phase 3 close-out](../../plans/test-coverage-improvement-plan.md#close-out-decisions-and-ci-handoff-2026-11-09).

Do not run coverage collection concurrently with another test process using the
same build output: Coverlet instruments those assemblies and can cause metadata
load failures in the other process. Run coverage separately, or use separate build
outputs. Container-isolation checks can run concurrently without coverage collection.
