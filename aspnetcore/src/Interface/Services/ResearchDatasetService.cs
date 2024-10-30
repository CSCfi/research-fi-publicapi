using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Query;
using ResearchFi.ResearchDataset;

namespace CSC.PublicApi.Interface.Services;

public class ResearchDatasetService : IResearchDatasetService
{
    private readonly ILogger<ResearchDatasetService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchService<ResearchDatasetSearchParameters, Service.Models.ResearchDataset.ResearchDataset> _searchService;

    public ResearchDatasetService(ILogger<ResearchDatasetService> logger, IMapper mapper, ISearchService<ResearchDatasetSearchParameters, Service.Models.ResearchDataset.ResearchDataset> searchService)
    {
        _logger = logger;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<ResearchDataset>, SearchResult)> GetResearchDatasets(GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, PaginationQueryParameters paginationQueryParameters)
    {
        var searchParameters = _mapper.Map<ResearchDatasetSearchParameters>(researchDatasetsQueryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, paginationQueryParameters.PageNumber, paginationQueryParameters.PageSize);

        return (_mapper.Map<IEnumerable<ResearchDataset>>(result), searchResult);
    }

    public async Task<(IEnumerable<ResearchDataset>, SearchAfterResult)> GetResearchDatasetsSearchAfter(GetResearchDatasetsQueryParameters queryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var searchParameters = _mapper.Map<ResearchDatasetSearchParameters>(queryParameters);

        var (result, searchAfterResult) = await _searchService.SearchAfter(searchParameters, searchAfterQueryParameters.PageSize, searchAfterQueryParameters.NextPageToken);

        return (_mapper.Map<IEnumerable<ResearchDataset>>(result), searchAfterResult);
    }
}