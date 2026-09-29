# Project Guidelines

research.fi Public API — a .NET 10 backend providing public REST APIs for Finnish research data (funding calls, funding decisions, organizations, publications, research datasets, infrastructures).

## Architecture

Data flow: SQL Server → `Repositories` (EF Core via `DatabaseContext`) → `Indexer` (console app, transforms to `Service.Models`) → Elasticsearch (via `ElasticService`/NEST) → `Interface` (ASP.NET Core Web API, maps to `ApiModels` via AutoMapper) → public API consumers.

Production projects live under `aspnetcore/src/`:
- **Interface** — the public-facing ASP.NET Core Web API (controllers, versioned V1/V2, Swagger).
- **Indexer** — console app that reads from SQL Server and indexes documents into Elasticsearch.
- **Exporter** — console app that exports Elasticsearch documents to JSON files on disk.
- **Repositories** — data access layer querying SQL Server via `DatabaseContext`.
- **DatabaseContext** — EF Core model, auto-scaffolded from SQL Server; see [aspnetcore/src/DatabaseContext/readme.md](aspnetcore/src/DatabaseContext/readme.md). Don't hand-edit generated entities.
- **Service.Models** — internal DTOs used between Repositories/Indexer.
- **ApiModels** — public API response models (namespace `CSC.PublicApi.ApiModels`).
- **ElasticService** — NEST-based abstraction for indexing/search.
- **Logging** — shared Serilog HTTP sink formatters.

Namespace convention: `CSC.PublicApi.<ProjectName>` (e.g. `CSC.PublicApi.Interface`).

## Build and Test

Run from the `aspnetcore/` directory (or use the `build`/`publish`/`watch` VS Code tasks, which target `aspnetcore/PublicApi.sln`):

```bash
dotnet build PublicApi.sln /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary
dotnet test
dotnet watch run --project PublicApi.sln
```

Test projects live under `aspnetcore/test/` (`Interface.Tests`, `Indexer.Tests`, `Repository.Tests`). All use xUnit + FluentAssertions; `Interface.Tests` uses `WebApplicationFactory` for integration tests.

CI runs the same restore/build/test sequence in `aspnetcore/` on PRs to `devel`/`main` (see `.github/workflows/dotnet.yml`).

Local development happens on developers' own machines, which may be Mac or Windows — avoid OS-specific assumptions (paths, shell syntax, line endings) in scripts and tooling.

## Conventions

- `Nullable` and `ImplicitUsings` are enabled on all projects — avoid introducing nullable warnings.
- Services are auto-registered via Scrutor conventions rather than manual DI registration; follow the existing naming/interface pattern when adding a new service so it gets picked up.
- Object mapping between `Service.Models` and `ApiModels` goes through AutoMapper profiles, not manual mapping code.
- Don't manually edit `DatabaseContext` entities — they are scaffolded from the SQL Server schema.

## Security and Deployment

- Never hard-code secrets — they are provided via configuration (appsettings, environment variables, user secrets).
- Production API endpoints must be protected by authorization.
- Applications are run in OpenShift.