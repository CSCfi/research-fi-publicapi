---
name: update-mydata-contracts-package
description: "Use when updating, bumping, or replacing the CSC.ResearchFi.Mydata.PublicApiContracts NuGet package (ProfileDataRequest/ProfileDataResponse, namespace ResearchFi.PersonPublicApi, used by PersonController), or when a new .nupkg from the Mydata API repo is handed over."
---

# Update CSC.ResearchFi.Mydata.PublicApiContracts

The package provides `ProfileDataRequest`/`ProfileDataResponse` (namespace `ResearchFi.PersonPublicApi`) for `PersonController`. It is built from the `api.PublicApiContracts` project in the Mydata API repo. There is no package feed; the package is a local `.nupkg` in `packages/`, resolved through the `Local` source in `aspnetcore/nuget.config`.

## Procedure

1. The Mydata API repo owner bumps the package version, runs `dotnet pack`, and hands over the `.nupkg`. Ask the user for the file if it is not already in `packages/`.
2. Copy `CSC.ResearchFi.Mydata.PublicApiContracts.<version>.nupkg` into `packages/` (repo root). Keep the old `.nupkg` only if the user wants it.
3. Update the `<PackageReference Include="CSC.ResearchFi.Mydata.PublicApiContracts" Version="..." />` in [aspnetcore/src/Interface/Interface.csproj](../../../aspnetcore/src/Interface/Interface.csproj) to the new version.
4. From `aspnetcore/`, restore, build, and test:
   ```bash
   dotnet restore PublicApi.sln
   dotnet build PublicApi.sln /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary
   dotnet test
   ```
5. Fix any compile errors in consumers (`PersonController`, `IPersonBackendClient` and its implementation, tests) caused by contract changes.

## Reusing the same version number

NuGet caches packages by id and version, so restore will not pick up a rebuilt `.nupkg` with an unchanged version. Clear the cache first:

```bash
dotnet nuget locals global-packages --clear
```

Then run `dotnet restore PublicApi.sln` from `aspnetcore/`. Prefer a new version number when possible.

## Notes

- Swagger loads the package's XML docs via `api.PublicApiContracts.xml` (see `SwaggerConfiguration.cs`); the package must ship that file under `lib/net10.0/`.
- Only `Interface.csproj` references the package; `Exporter` and `Interface.Tests` get it transitively.
- Commands are cross-platform; avoid OS-specific shell syntax.
