using CSC.PublicApi.ElasticService;
using ResearchFi.Infrastructure;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IInfrastructureService
{
    Task<(IEnumerable<Infrastructure>, SearchResult)> GetInfrastructures(GetInfrastructuresQueryParameters infrastructuresQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<Infrastructure>, long? searchAfter)> GetInfrastructuresSearchAfter(GetInfrastructuresQueryParameters infrastructuresQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
}