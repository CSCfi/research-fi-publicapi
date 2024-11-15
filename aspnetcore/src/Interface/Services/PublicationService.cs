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

    public async Task<(IEnumerable<Publication>, SearchResult)> GetPublications(GetPublicationsQueryParameters publicationsQueryParameters, PaginationQueryParameters paginationQueryParameters)
    {
        var searchParameters = _mapper.Map<PublicationSearchParameters>(publicationsQueryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, paginationQueryParameters.PageNumber, paginationQueryParameters.PageSize);

        return (_mapper.Map<IEnumerable<Publication>>(result), searchResult);
    }

    public async Task<(IEnumerable<Publication>, SearchAfterResult)> GetPublicationsSearchAfter(GetPublicationsQueryParameters publicationsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var searchParameters = _mapper.Map<PublicationSearchParameters>(publicationsQueryParameters);

        var (result, searchAfterResult) = await _searchService.SearchAfter(searchParameters, searchAfterQueryParameters.PageSize, searchAfterQueryParameters.NextPageToken);

        return (_mapper.Map<IEnumerable<Publication>>(result), searchAfterResult);
    }

    public async Task<Publication?> GetPublication(string publicationId)
    {
        var result = await _searchService.GetSingle(publicationId);

        return _mapper.Map<Publication>(result);
    }
}