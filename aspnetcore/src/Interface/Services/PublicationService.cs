using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Publication;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public class PublicationService : IPublicationService
{
    private readonly IMapper _mapper;
    private readonly ISearchService<PublicationSearchParameters, Service.Models.Publication.Publication> _searchService;

    public PublicationService(IMapper mapper, ISearchService<PublicationSearchParameters, Service.Models.Publication.Publication> searchService)
    {
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<Publication>, SearchResult)> GetPublications(GetPublicationsQueryParameters queryParameters)
    {
        var searchParameters = _mapper.Map<PublicationSearchParameters>(queryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, queryParameters.PageNumber, queryParameters.PageSize);

        return (_mapper.Map<IEnumerable<Publication>>(result), searchResult);
    }

    public async Task<Publication?> GetPublication(string publicationId)
    {
        var result = await _searchService.GetSingle(publicationId);

        return _mapper.Map<Publication>(result);
    }
}