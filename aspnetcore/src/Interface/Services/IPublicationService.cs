using CSC.PublicApi.ElasticService;
using ResearchFi.Publication;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IPublicationService
{
    Task<(IEnumerable<Publication>, SearchResult)> GetPublications(GetPublicationsQueryParameters queryParameters);
    
    Task<Publication?> GetPublication(string publicationId);
}