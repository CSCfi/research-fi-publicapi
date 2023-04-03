using CSC.PublicApi.ElasticService;
using ResearchFi.Organization;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IOrganizationService
{
    Task<(IEnumerable<Organization>, SearchResult)> GetOrganizations(GetOrganizationsQueryParameters queryParameters);
}