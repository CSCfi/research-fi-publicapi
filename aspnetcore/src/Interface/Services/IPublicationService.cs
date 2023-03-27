using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Publication;

namespace CSC.PublicApi.Interface.Services;

public interface IPublicationService
{
    Task<(IEnumerable<Publication>, SearchResult)> GetPublications(GetPublicationsQueryParameters queryParameters);
    
    Task<Publication?> GetPublication(string publicationId);
}