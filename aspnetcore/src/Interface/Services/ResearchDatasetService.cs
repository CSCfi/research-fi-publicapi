using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.ResearchDataset;

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

    public async Task<(IEnumerable<ResearchDataset>, SearchResult)> GetResearchDatasets(GetResearchDatasetsQueryParameters queryParameters)
    {
        var searchParameters = _mapper.Map<ResearchDatasetSearchParameters>(queryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, queryParameters.PageNumber, queryParameters.PageSize);

        return (_mapper.Map<IEnumerable<ResearchDataset>>(result), searchResult);
    }
}