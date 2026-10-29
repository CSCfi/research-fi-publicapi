using System.Net;
using ResearchFi.ProfilePublicApi;

namespace CSC.PublicApi.Interface.Services;

public interface IPersonBackendClient
{
    Task<(ProfileDataResponse? Response, HttpStatusCode? ErrorStatusCode, string? ErrorContent, string? ErrorContentType)> SearchPersonsAsync(ProfileDataRequest request, string? clientId, CancellationToken cancellationToken = default);
}
