using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IPersonBackendClient
{
    Task<string?> SearchPersonsAsync(GetPersonsQueryParameters queryParameters, CancellationToken cancellationToken = default);
}
