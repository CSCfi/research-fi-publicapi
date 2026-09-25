using ResearchFi.PersonPublicApi;

namespace CSC.PublicApi.Interface.Services;

public interface IPersonBackendClient
{
    Task<ProfileDataResponse?> SearchPersonsAsync(ProfileDataRequest request, string? clientId, CancellationToken cancellationToken = default);
}
