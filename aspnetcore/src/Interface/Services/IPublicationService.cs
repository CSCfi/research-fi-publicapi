using CSC.PublicApi.ElasticService;
using ResearchFi.Publication;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IPublicationService
{
    Task<(IEnumerable<Publication>, SearchResult)> GetPublications(GetPublicationsQueryParameters publicationsQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<Publication>, long? searchAfter)> GetPublicationsSearchAfter(GetPublicationsQueryParameters publicationsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
    
    Task<Publication?> GetPublication(string publicationId);
}