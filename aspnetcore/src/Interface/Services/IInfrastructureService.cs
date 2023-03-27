using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Infrastructure;

namespace CSC.PublicApi.Interface.Services;

public interface IInfrastructureService
{
    Task<(IEnumerable<Infrastructure>, SearchResult)> GetInfrastructures(GetInfrastructuresQueryParameters queryParameters);
}