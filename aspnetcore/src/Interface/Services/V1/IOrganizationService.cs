/*
 * API version 1.0
 */
using CSC.PublicApi.ElasticService;
using ResearchFi.Organization;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services.V1;

public interface IOrganizationService
{
    Task<(IEnumerable<Organization>, SearchResult)> GetOrganizations(GetOrganizationsQueryParameters organizationsQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<Organization>, SearchAfterResult)> GetOrganizationsSearchAfter(GetOrganizationsQueryParameters organizationsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
}