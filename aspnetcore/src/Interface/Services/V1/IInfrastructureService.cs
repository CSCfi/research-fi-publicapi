/*
 * API version 1.0
 */
using CSC.PublicApi.ElasticService;
using ResearchFi.Infrastructure;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services.V1;

public interface IInfrastructureService
{
    Task<(IEnumerable<Infrastructure>, SearchResult)> GetInfrastructures(GetInfrastructuresQueryParameters infrastructuresQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<Infrastructure>, SearchAfterResult)> GetInfrastructuresSearchAfter(GetInfrastructuresQueryParameters infrastructuresQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
    Task<Infrastructure?> GetInfrastructure(string urn);
}