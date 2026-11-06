using System.Net;
using ResearchFi.ProfilePublicApi;

namespace CSC.PublicApi.Interface.Services;

public interface IProfileBackendClient
{
    Task<(ProfileDataResponse? Response, HttpStatusCode? ErrorStatusCode, string? ErrorContent, string? ErrorContentType)> SearchProfilesAsync(ProfileDataRequest request, string? clientId, CancellationToken cancellationToken = default);
}
