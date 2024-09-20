using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Infrastructure;
using ResearchFi.Query;

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

    public async Task<(IEnumerable<Infrastructure>, SearchResult)> GetInfrastructures(GetInfrastructuresQueryParameters infrastructuresQueryParameters, PaginationQueryParameters paginationQueryParameters)
    {
        var searchParameters = _mapper.Map<InfrastructureSearchParameters>(infrastructuresQueryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, paginationQueryParameters.PageNumber, paginationQueryParameters.PageSize);

        return (_mapper.Map<IEnumerable<Infrastructure>>(result), searchResult);
    }


    public async Task<(IEnumerable<Infrastructure>, long? searchAfter)> GetInfrastructuresSearchAfter(GetInfrastructuresQueryParameters infrastructuresQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var searchParameters = _mapper.Map<InfrastructureSearchParameters>(infrastructuresQueryParameters);

        var (result, searchAfter) = await _searchService.SearchAfter(searchParameters, searchAfterQueryParameters.PageSize, searchAfterQueryParameters.NextPageToken);

        return (_mapper.Map<IEnumerable<Infrastructure>>(result), searchAfter);
    }
}