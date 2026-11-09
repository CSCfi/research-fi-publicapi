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

[FundingCallSchema.sql](Repository.Tests/SqlServer/FundingCallSchema.sql) and
[FundingDecisionSchema.sql](Repository.Tests/SqlServer/FundingDecisionSchema.sql) are
versioned, test-only subsets of the approved scaffolded
[ApiDbContext](../src/DatabaseContext/ApiDbContext.cs) mappings, applied in that order.
They include the columns, keys, and relationships used by the funding-call and
funding-decision repository queries, not the entire warehouse or all its constraints.
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

The timestamp characterization tests record an existing production behavior: null
`created` or `modified` values in a funding decision cause SQL error 242 during the
projection from nullable warehouse timestamps to non-nullable model timestamps.
This is not considered successful data retrieval. The maintainer accepted retaining
this behavior for Phase 3 (P3-TIME); repository maintainers must revisit it before
Phase 4 acceptance and agree a timestamp contract before a separate production fix.
Other scenarios seed explicit valid timestamps.

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
jobs upload TRX and coverage artifacts even on test failure. Their Linux CI execution
and repeated-run stability must be verified before making them required branch checks.
The maintainer will commit/push and run CI. Phase 3 implementation is complete with
accepted exceptions, but CI acceptance remains open: retain evidence of at least ten
consecutive clean Elasticsearch runs and parallel isolation, plus repeated clean SQL
runs on Linux x86-64. See the [Phase 3 close-out](../../plans/test-coverage-improvement-plan.md#close-out-decisions-and-ci-handoff-2026-11-09).

Do not run coverage collection concurrently with another test process using the
same build output: Coverlet instruments those assemblies and can cause metadata
load failures in the other process. Run coverage separately, or use separate build
outputs. Container-isolation checks can run concurrently without coverage collection.
