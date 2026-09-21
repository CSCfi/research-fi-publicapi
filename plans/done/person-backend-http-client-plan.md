Status: Implemented
2026-09-21

# Person backend HTTP client scaffolding — summary

`PersonController.PostPerson` (POST `/v1/persons`) now forwards `GetPersonsQueryParameters` as a
JSON body to an external backend (owned by the same business project, source in another repo)
instead of querying Elasticsearch/SQL Server. Added a typed `HttpClient` client/interface,
externally-configurable base URL, and wired the controller to call it. Response is currently the
raw response body (`string?`, may be null) — no typed response DTO yet, to be refined once the
other backend's contract is confirmed.

**Files touched:**
- `aspnetcore/src/Interface/Services/V1/IPersonBackendClient.cs`, `PersonBackendClient.cs` (new)
- `aspnetcore/src/Interface/Configuration/PersonBackendSettings.cs` (new) + `ConfigurationExtensions.cs`
- `aspnetcore/src/Interface/appsettings.json` (`PersonBackend:BaseUrl`, empty — set per environment)
- `aspnetcore/src/Interface/Program.cs` — `AddHttpClient<IPersonBackendClient, PersonBackendClient>()`
- `aspnetcore/src/Interface/Controllers/V1/PersonController.cs`

**Gotcha worth remembering:** the project's Scrutor convention scan
(`services.Scan(...).AddClasses().AsMatchingInterface().WithScopedLifetime()`) would also match
`PersonBackendClient : IPersonBackendClient` and register it as a plain scoped service without the
configured `HttpClient`. The `AddHttpClient<...>()` call must run *after* that scan so it wins DI
resolution — otherwise the typed client's `BaseAddress`/factory-managed lifetime silently doesn't apply.

**Open follow-ups (not yet resolved):**
- Actual contract with the external backend (path/verb, body vs. query, response schema) is assumed
  (`POST {BaseUrl}/persons/search`) and unconfirmed.
- Whether the external backend needs its own auth (API key/client-credentials) vs. forwarding the
  caller's bearer token.
- Whether failures should map to a specific status (e.g. `502`) instead of swallowing as `null`.
- No retry/timeout/circuit-breaker policy yet (candidate: `Microsoft.Extensions.Http.Resilience`).
- `Interface.Tests` needs a way to substitute `IPersonBackendClient` to avoid real network calls.
