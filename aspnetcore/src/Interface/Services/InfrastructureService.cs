using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Infrastructure;

namespace CSC.PublicApi.Interface.Services;

public class InfrastructureService : IInfrastructureService
{
    private readonly ILogger<InfrastructureService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchService<InfrastructureSearchParameters, Service.Models.Infrastructure.Infrastructure> _searchService;

    public InfrastructureService(ILogger<InfrastructureService> logger, IMapper mapper, ISearchService<InfrastructureSearchParameters, Service.Models.Infrastructure.Infrastructure> searchService)
    {
        _logger = logger;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<Infrastructure>, SearchResult)> GetInfrastructures(GetInfrastructuresQueryParameters queryParameters)
    {
        var searchParameters = _mapper.Map<InfrastructureSearchParameters>(queryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, queryParameters.PageNumber, queryParameters.PageSize);

        return (_mapper.Map<IEnumerable<Infrastructure>>(result), searchResult);
    }
}