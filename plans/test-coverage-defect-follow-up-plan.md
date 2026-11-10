Status: Findings recorded; implementation not authorized
2026-11-10

# Test coverage findings: production defect follow-up plan

## Scope and workflow

This plan tracks production defects discovered while implementing the
[test coverage improvement plan](test-coverage-improvement-plan.md).
That workstream changes tests, test infrastructure, CI, and related documentation;
production changes are limited to removing confirmed unused code. Defect fixes
must not be implemented as part of the coverage plan.

For each new finding, record the observed behavior, evidence, impact, affected
surface, test disposition, owner, and required contract decision and validation.
Distinguish confirmed defects from suspected problems that still need evidence.
Do not treat characterization of a failure as approval of that behavior.

Repository maintainers own follow-up. On 2026-11-10, the user accepted the
findings below as time-bounded Phase 4 coverage exceptions; this does not approve
the behavior as correct or authorize production fixes. Review each disposition
by 2026-12-10, prioritize and approve fixes separately, then implement and
validate them in a dedicated follow-up change.

| Finding | Phase 4 disposition | Owner | Review by |
| --- | --- | --- | --- |
| P3-ORG | Keep the two organization search cases skipped; do not certify organization name search as working. | Repository maintainers | 2026-12-10 |
| P3-TIME | Retain active SQL failure characterization; agree the timestamp contract before a production fix. | Repository maintainers | 2026-12-10 |
| P4-PUB-CONTRIB | Retain characterization pending agreement on the contribution identity key. | Repository maintainers | 2026-12-10 |
| P4-SEARCH | Retain failure characterization; agree how search failures surface to API/export consumers. | Repository maintainers | 2026-12-10 |
| P4-COUNT | Retain count-failure characterization; agree the export-total/error contract. | Repository maintainers | 2026-12-10 |
| P4-ROTATE | Retain lifecycle failure characterization; agree safe health/alias/delete transition requirements. | Repository maintainers | 2026-12-10 |
| P4-PROFILE | Retain null-success characterization; agree the response contract for an empty backend result. | Repository maintainers | 2026-12-10 |
| P4-LINK | Retain empty-pagination link characterization; agree whether page-zero links are valid. | Repository maintainers | 2026-12-10 |

Phase 4 acceptance still requires the latest changes to pass both integration
lanes on supported Linux x86-64 CI. The earlier CI results documented elsewhere
predate the latest coverage additions and do not satisfy that final validation.

## Findings

### P3-ORG - Organization search field-name mismatch

- **Status:** Confirmed in Phase 3; accepted coverage exception, fix deferred.
- **Owner:** Repository maintainers.
- **Surface:** [OrganizationQueryGenerator](../aspnetcore/src/ElasticService/ElasticSearchQueryGenerators/OrganizationQueryGenerator.cs)
  and production Elasticsearch mappings.
- **Evidence:** Synthetic integration searches exposed queries using lowercase
  `nameFi`/`nameSv`/`nameEn` while the configured index maps PascalCase properties.
- **Impact:** Organization name searches can miss matching indexed documents.
  The current coverage does not certify this search as working.
- **Test disposition:** The positive and negative organization cases in
  [ElasticsearchSearchSystemTest](../aspnetcore/test/Interface.Tests/ElasticsearchSearchSystemTest.cs)
  remain explicitly skipped. Do not change production mapping or queries in the
  coverage workstream to enable them.
- **Follow-up decision:** Review query field names against the production
  serializer and index mapping; agree a correction and any index compatibility
  or deployment implications.
- **Future validation:** Reactivate both cases against production AutoMap
  settings, verify matching and nonmatching names, and preserve authorization
  and intentional Swagger visibility.

### P3-TIME - Funding-decision null-timestamp projection

- **Status:** Confirmed in Phase 3; accepted coverage exception, fix deferred.
- **Owner:** Repository maintainers.
- **Surface:** [FundingDecisionProfile](../aspnetcore/src/Repositories/Maps/FundingDecisionProfile.cs)
  and [FundingDecisionIndexRepository](../aspnetcore/src/Repositories/FundingDecisionIndexRepository.cs).
- **Evidence:** Nullable warehouse `created` or `modified` projected to
  non-nullable model timestamps causes SQL error 242.
  [FundingDecisionSqlServerTest](../aspnetcore/test/Repository.Tests/FundingDecisionSqlServerTest.cs)
  reproduces each case in an isolated SQL Server transaction that is rolled back.
- **Impact:** A funding decision with either timestamp missing can fail
  relational retrieval instead of producing an indexable result.
- **Test disposition:** Keep both characterization cases active and clearly
  labelled as current failure behavior. Other scenarios use explicit valid
  timestamps; no fallback timestamp or model change is authorized here.
- **Follow-up decision:** Agree the source/model timestamp contract, including
  whether absence is valid and how it should be represented. Do not invent a
  sentinel or substitute date merely to make projection succeed.
- **Future validation:** Test null `created`, null `modified`, both null, and
  valid timestamps against SQL Server using production projection; verify the
  agreed downstream model, indexing, and public response behavior.

### P4-PUB-CONTRIB - Co-publication contribution deduplication uses object identity

- **Status:** Confirmed in Phase 4; intended deduplication key requires maintainer review.
- **Owner:** Repository maintainers; review by 2026-12-10.
- **Surface:** `PerformInMemoryOperation` in
  [PublicationIndexRepository](../aspnetcore/src/Repositories/PublicationIndexRepository.cs).
- **Evidence:** The active characterization case in
  [PublicationIndexRepositoryTest](../aspnetcore/test/Repository.Tests/PublicationIndexRepositoryTest.cs)
  supplies related contribution rows with distinct `Name` instances but the same
  `NameId`. `DistinctBy(x => x.Name)` retains both because `Name` does not define
  value equality.
- **Impact:** Equivalent contributors can remain duplicated in the aggregated
  `DatabaseContributions`; downstream author/organization aggregation may be
  affected depending on the contribution types and relationships.
- **Test disposition:** Keep the explicitly named characterization case. Existing
  coverage still verifies removal when DTO rows share the same `Name` instance.
  No production change is included in the coverage workstream.
- **Follow-up decision:** Confirm whether co-publication contributions should be
  unique by `NameId`, by person plus contribution role/organization, or by another
  domain key before selecting a correction.
- **Future validation:** Verify repeated rows for the same identity across
  related publications, while preserving distinct roles and organization/unit
  affiliations under the agreed key; check resulting authors and organizations.

### P4-SEARCH - Failed Elasticsearch searches look like empty results

- **Status:** Confirmed in Phase 4; fix deferred to this follow-up workstream.
- **Owner:** Repository maintainers; review by 2026-12-10.
- **Surface:** `Search`, `GetSingle`, and `SearchAfter` in
  [ElasticSearchService](../aspnetcore/src/ElasticService/ElasticSearchService.cs).
- **Evidence:** Active HTTP 400 and 503 response cases in
  [ElasticSearchServiceTest](../aspnetcore/test/Interface.Tests/Services/ElasticSearchServiceTest.cs)
  use the real NEST client with its default non-throwing behavior and production
  field-name inference. The service does not check response validity.
  `Search` returns empty documents and zero total; `GetSingle` returns null.
  `SearchAfter` returns no documents or cursor and still issues a count, which
  can report a nonzero total despite the failed search.
- **Impact:** Callers cannot distinguish a failed search from no matches or a
  missing publication; export traversal can appear finished despite failed
  retrieval.
- **Test disposition:** Keep explicitly named characterization cases for current
  behavior and separate cases showing propagation when NEST is configured to
  throw. The test transport disables retries for deterministic final-response
  simulation; it does not certify production retry behavior. No production
  client settings or service behavior have been changed.
- **Follow-up decision:** Agree how unsuccessful search responses are surfaced
  through the search service and HTTP/export consumers, including logging and
  the distinction between absent data and failed retrieval.
- **Future validation:** HTTP query errors and service unavailability must not
  become successful empty results or not-found responses. Verify the approved
  failure contract, successful empty results, retry exhaustion, and that failed
  export retrieval cannot indicate successful completion.

### P4-COUNT - Export count failures return misleading or suppressed totals

- **Status:** Confirmed in Phase 4; fix deferred to this follow-up workstream.
- **Owner:** Repository maintainers; review by 2026-12-10.
- **Surface:** `SearchAfter` in
  [ElasticSearchService](../aspnetcore/src/ElasticService/ElasticSearchService.cs).
- **Evidence:** HTTP 400/503 count-response cases in
  [ElasticSearchServiceTest](../aspnetcore/test/Interface.Tests/Services/ElasticSearchServiceTest.cs)
  show two paths after a successful search. With default non-throwing NEST
  settings, the invalid count response is not checked and `TotalResults` becomes
  zero even when documents were returned. With throwing settings, the broad
  catch suppresses the exception and returns a null total without reporting it.
- **Impact:** Export totals can incorrectly state zero matching documents or
  silently become unknown; successful document retrieval masks the count failure.
- **Test disposition:** Both paths are active, labelled characterization cases,
  not desired error-handling contracts. Valid count totals above 10,000 and
  search/count filter parity are covered separately.
- **Follow-up decision:** Decide whether count failure fails the operation or
  permits explicitly reported partial metadata, and define logging and HTTP
  behavior. Do not equate an unsuccessful count with a verified zero.
- **Future validation:** Cover invalid count responses, thrown client/transport
  errors, genuine zero counts, and nonzero counts. Verify the agreed observable
  error or partial-result contract without silent suppression.

### P4-ROTATE - Unchecked index lifecycle responses permit unsafe transitions

- **Status:** Confirmed in Phase 4; fix deferred.
- **Owner:** Repository maintainers; review by 2026-12-10.
- **Surface:** `SwitchIndexes`, `CreateIndex`, and `GetIndexNames` in
  [ElasticSearchIndexService](../aspnetcore/src/ElasticService/ElasticSearchIndexService.cs).
- **Evidence:** Active characterization cases in
  [ElasticSearchIndexBoundaryTest](../aspnetcore/test/Interface.Tests/Services/ElasticSearchIndexBoundaryTest.cs)
  use real NEST response parsing with default non-throwing settings and retries
  disabled. `SwitchIndexes` proceeds to alias mutation and old-index deletion
  after HTTP 503 health failure or a red/timed-out HTTP 200 health response.
  It requests old-index deletion after an HTTP 503 alias failure or an
  unacknowledged HTTP 200 alias response. A failed old-index deletion returns
  normally. `CreateIndex` attempts creation after failed pre-deletion;
  `GetIndexNames` treats a failed existence request as absence. None of these
  cases produces error-level logging from the service.
- **Impact:** Unsuccessful readiness or alias changes can be followed by
  destructive requests against the previous index. Failed existence checks can
  select the wrong rotation target; failed cleanup is presented as completion.
  Tests prove request sequencing, not actual data loss in a real cluster.
- **Test disposition:** Keep clearly named characterization cases and the
  allowed-404 deletion control. Separate tests verify that rejected or
  unacknowledged index creation and bulk item/HTTP failures stop later work.
  Production lifecycle behavior is unchanged by the coverage workstream.
- **Follow-up decision:** Define required health, validity, and acknowledgement
  checks, safe stop/recovery behavior, and observable failure reporting for each
  lifecycle transition. Preserve intentional idempotent deletion of absent indices.
- **Future validation:** Failed existence/readiness/alias operations must not
  trigger unsafe subsequent mutations under the agreed contract. Verify both
  non-throwing and throwing client paths, deletion failures, successful rotation,
  allowed missing-index deletion, and preservation of the serving index.

### P4-PROFILE - Null profile backend success becomes HTTP 204

- **Status:** Confirmed in Phase 4; fix deferred.
- **Owner:** Repository maintainers; review by 2026-12-10.
- **Surface:** [ProfileBackendClient](../aspnetcore/src/Interface/Services/ProfileBackendClient.cs)
  and [ProfileController](../aspnetcore/src/Interface/Controllers/V1/ProfileController.cs).
- **Evidence:** An HTTP 200 backend response containing JSON `null` deserializes
  into a null `ProfileDataResponse` with no error status/content. The controller
  returns `Ok(null)`, which the HTTP pipeline emits as 204 No Content.
  Active characterization cases in
  [ProfileBackendContractTest](../aspnetcore/test/Interface.Tests/Services/ProfileBackendContractTest.cs)
  and [ProfileEndpointContractTest](../aspnetcore/test/Interface.Tests/ProfileEndpointContractTest.cs)
  reproduce both layers. Empty, malformed, and array success payloads instead
  throw `JsonException` in the backend client.
- **Impact:** A missing backend contract is indistinguishable from an intentional
  successful no-content response, rather than returning profile/consent status.
- **Test disposition:** Keep both null-payload characterization cases active,
  without endorsing 204 as the desired profile contract. No production fix is
  included in coverage work.
- **Follow-up decision:** Confirm whether null/no-content success is permitted
  by the backend contract; define explicit handling and observable reporting for
  missing or invalid success payloads.
- **Future validation:** Cover valid profile responses, JSON null, empty content,
  backend 204, malformed JSON, and incompatible payload shapes through both the
  client and HTTP pipeline under the approved contract.

### P4-LINK - Empty pagination links to page zero

- **Status:** Confirmed in Phase 4; fix deferred.
- **Owner:** Repository maintainers; review by 2026-12-10.
- **Surface:** `AddPaginationResponseHeaders` in
  [ResponseHelper](../aspnetcore/src/Interface/Controllers/ResponseHelper.cs).
- **Evidence:** With page 1, page size 10, and zero results, total pages is zero.
  The helper unconditionally emits a `last` link with `pageNumber=0`, alongside
  a `first` link to page 1. The active characterization case in
  [ResponseHelperContractTest](../aspnetcore/test/Interface.Tests/ResponseHelperContractTest.cs)
  asserts the exact current links.
- **Impact:** Empty responses advertise a destination outside the API's
  one-based pagination contract. Clients following it may have the page
  normalized rather than reaching a distinct valid last page.
- **Test disposition:** Keep the explicitly named characterization case; positive
  first/middle/last-page link and filter-preservation contracts remain separate.
  No production correction is included in coverage work.
- **Follow-up decision:** Agree whether empty responses should omit navigation
  links or consistently target page 1.
- **Future validation:** Check empty and nonempty pagination at the helper and
  HTTP layers, ensuring every emitted page number is valid and filters survive.
