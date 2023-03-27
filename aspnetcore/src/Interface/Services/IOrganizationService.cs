using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Organization;

namespace CSC.PublicApi.Interface.Services;

public interface IOrganizationService
{
    Task<(IEnumerable<Organization>, SearchResult)> GetOrganizations(GetOrganizationsQueryParameters queryParameters);
}