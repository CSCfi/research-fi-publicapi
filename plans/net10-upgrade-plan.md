# .NET 8 → .NET 10 Upgrade Plan

_Created: 2026-09-08_

## Scope & Approach
Minimum-effort path to a working build/test on `net10.0` (no intermediate net9 step). Out of scope: NEST/Elasticsearch client migration, `Microsoft.AspNetCore.Mvc.Versioning` replacement, cleanup of stale/unreferenced folders (`src/Api`, `src/Api.Test`, `src/ElasticSearchIndexer`), deployment/OpenShift validation.

## 1. Update TargetFramework
Change `<TargetFramework>net8.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>` in all 12 project files:
- src/ApiModels/ApiModels.csproj
- src/DatabaseContext/DatabaseContext.csproj
- src/ElasticService/ElasticService.csproj
- src/Exporter/Exporter.csproj
- src/Indexer/Indexer.csproj
- src/Interface/Interface.csproj
- src/Logging/Logging.csproj
- src/Repositories/Repositories.csproj
- src/Service.Models/Service.Models.csproj
- test/Indexer.Tests/Indexer.Tests.csproj
- test/Interface.Tests/Interface.Tests.csproj
- test/Repository.Tests/Repository.Tests.csproj

## 2. Bump framework-versioned NuGet packages to 10.x
Only packages whose major version tracks the target framework need bumping (leave everything else, e.g. NEST, AutoMapper, Swashbuckle, Serilog, Scrutor, Mvc.Versioning, xunit, Moq, untouched):
- `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Design`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`: 8.0.8 → 10.0.x (latest stable)
- `Microsoft.AspNetCore.Authentication.JwtBearer`, `Microsoft.AspNetCore.Authentication.OpenIdConnect`, `Microsoft.AspNetCore.HeaderPropagation`: 8.0.8 → 10.0.x
- `Microsoft.Extensions.Hosting`, `Microsoft.Extensions.Hosting.Abstractions`: 8.0.0 → 10.0.0
- `Microsoft.AspNetCore.Mvc.Testing`: 8.0.8 → 10.0.x (Interface.Tests)

## 3. Dockerfiles
- `openshift/api/rahti2/Dockerfile`: `sdk:8.0` → `sdk:10.0`, `aspnet:8.0` → `aspnet:10.0`
- `openshift/indexer/rahti2/Dockerfile`: `sdk:8.0` → `sdk:10.0`, `runtime:8.0` → `runtime:10.0`

## 4. CI
- `.github/workflows/dotnet.yml`: `dotnet-version: 8.0.x` → `10.0.x`
- `.github/workflows/codeql.yml`: no explicit SDK pin, no change needed (uses Autobuild)

## 5. Build & fix
1. `dotnet restore` / `dotnet build aspnetcore/PublicApi.sln`
2. Resolve any compiler errors/warnings from removed/changed APIs (expect minimal, given no other major package bumps)
3. `dotnet test` across all three test projects
4. Fix any test breakages surfaced by the EF Core / Auth package bumps

## 6. Done criteria
- Solution builds clean on net10.0
- All existing tests pass
- CI workflow green
