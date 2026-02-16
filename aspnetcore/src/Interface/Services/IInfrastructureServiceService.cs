using CSC.PublicApi.ElasticService;
using ResearchFi.Infrastructure;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IInfrastructureServiceService
{
    Task<(IEnumerable<ResearchFi.Infrastructure.Service>, SearchResult)> GetInfrastructureServices(GetInfrastructureServicesQueryParameters infrastructureServicesQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<ResearchFi.Infrastructure.Service>, SearchAfterResult)> GetInfrastructureServicesSearchAfter(GetInfrastructureServicesQueryParameters infrastructureServicesQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
    Task<ResearchFi.Infrastructure.Service?> GetInfrastructureService(string urn);
}