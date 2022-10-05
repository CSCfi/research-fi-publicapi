using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Publication;

namespace CSC.PublicApi.Interface.Services;

public class PublicationService : IPublicationService
{
    private readonly ILogger<PublicationService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchService<PublicationSearchParameters, Service.Models.Publication.Publication> _searchService;

    public PublicationService(ILogger<PublicationService> logger, IMapper mapper, ISearchService<PublicationSearchParameters, Service.Models.Publication.Publication> searchService)
    {
        _logger = logger;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<Publication>, SearchResult)> GetPublications(GetPublicationsQueryParameters queryParameters)
    {
        var searchParameters = _mapper.Map<PublicationSearchParameters>(queryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, queryParameters.PageNumber, queryParameters.PageSize);

        return (_mapper.Map<IEnumerable<Publication>>(result), searchResult);
    }
}