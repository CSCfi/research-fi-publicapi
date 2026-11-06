Status: Implemented
2026-09-21

# Profile endpoint (V1) — summary

Added a minimal `ProfileController` under `Interface/Controllers/V1`, following existing
controller conventions (versioning, auth policy, diagnostic logging), with no new
`ApiModels`/`Service.Models`/service classes. Later superseded/extended by the
profile-backend-http-client work (see [profile-backend-http-client-plan.md](profile-backend-http-client-plan.md)),
which turned the initial `501` placeholder into a real call to an external backend and
renamed the policy to `Profile.Read` (the endpoint is POST but read-only —
POST is used only to keep search parameters out of the URL).

**Files touched:**
- `aspnetcore/src/Interface/Controllers/V1/ProfileController.cs`
- `aspnetcore/src/Interface/ApiConstants.cs` (`LogResourceType_Profile`)
- `aspnetcore/src/Interface/ApiPolicies.cs` (`Profile.Read` policy, `profile-read` role claim)

**Note:** the `profile-read` role still needs to be added to Keycloak out-of-band.
