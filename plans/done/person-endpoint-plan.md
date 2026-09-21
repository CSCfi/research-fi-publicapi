Status: Implemented
2026-09-21

# Person endpoint (V1) — summary

Added a minimal `PersonController` under `Interface/Controllers/V1`, following existing
controller conventions (versioning, auth policy, diagnostic logging), with no new
`ApiModels`/`Service.Models`/service classes. Later superseded/extended by the
person-backend-http-client work (see [person-backend-http-client-plan.md](person-backend-http-client-plan.md)),
which turned the initial `501` placeholder into a real call to an external backend and
renamed the policy from `Person.Write` to `Person.Read` (the endpoint is POST but read-only —
POST is used only to keep search parameters out of the URL).

**Files touched:**
- `aspnetcore/src/Interface/Controllers/V1/PersonController.cs`
- `aspnetcore/src/Interface/ApiConstants.cs` (`LogResourceType_Person`)
- `aspnetcore/src/Interface/ApiPolicies.cs` (`Person.Read` policy, `person-read` role claim)

**Note:** the `person-read` role still needs to be added to Keycloak out-of-band.
