Status: Implemented
2026-09-21

# Profile backend HTTP client scaffolding — summary

`ProfileController.PostProfile` (POST `/v1/profiles`) forwards `ProfileDataRequest` as a JSON body
to the external profile backend instead of querying Elasticsearch/SQL Server. The controller
uses the typed `IProfileBackendClient`/`ProfileBackendClient` client and returns a
`ProfileDataResponse`.

**Files touched:**
- `aspnetcore/src/Interface/Services/IProfileBackendClient.cs`, `ProfileBackendClient.cs` (new)
- `aspnetcore/src/Interface/Configuration/ProfileBackendSettings.cs` (new) + `ConfigurationExtensions.cs`
- `aspnetcore/src/Interface/appsettings.json` (`ProfileBackend:BaseUrl`, empty — set per environment)
- `aspnetcore/src/Interface/Program.cs` — `AddHttpClient<IProfileBackendClient, ProfileBackendClient>()`
- `aspnetcore/src/Interface/Controllers/V1/ProfileController.cs`

**Gotcha worth remembering:** the project's Scrutor convention scan
(`services.Scan(...).AddClasses().AsMatchingInterface().WithScopedLifetime()`) would also match
`ProfileBackendClient : IProfileBackendClient` and register it as a plain scoped service without the
configured `HttpClient`. The `AddHttpClient<...>()` call must run *after* that scan so it wins DI
resolution — otherwise the typed client's `BaseAddress`/factory-managed lifetime silently doesn't apply.

**Open follow-ups (not yet resolved):**
- The current client posts `ProfileDataRequest` to `{BaseUrl}/api/publicapi/profile` and reads
  `ProfileDataResponse`; contract changes should be coordinated with the Mydata API package.
- Whether the external backend needs its own auth (API key/client-credentials) vs. forwarding the
  caller's bearer token.
- Whether failures should map to a specific status (e.g. `502`) instead of swallowing as `null`.
- No retry/timeout/circuit-breaker policy yet (candidate: `Microsoft.Extensions.Http.Resilience`).
- `Interface.Tests` needs a way to substitute `IProfileBackendClient` to avoid real network calls.
