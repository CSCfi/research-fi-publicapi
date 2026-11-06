using System.Net;
using System.Net.Http.Json;
using ResearchFi.ProfilePublicApi;

namespace CSC.PublicApi.Interface.Services;

public class ProfileBackendClient : IProfileBackendClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProfileBackendClient> _logger;

    public ProfileBackendClient(HttpClient httpClient, ILogger<ProfileBackendClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(ProfileDataResponse? Response, HttpStatusCode? ErrorStatusCode, string? ErrorContent, string? ErrorContentType)> SearchProfilesAsync(ProfileDataRequest request, string? clientId, CancellationToken cancellationToken = default)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/publicapi/profile")
        {
            Content = JsonContent.Create(request)
        };
        if (clientId != null)
        {
            httpRequest.Headers.Add("public-api-clientid", clientId);
        }
        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Profile backend request failed with status code {StatusCode}", response.StatusCode);
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var errorContentType = response.Content.Headers.ContentType?.ToString();
            return (null, response.StatusCode, errorContent, errorContentType);
        }

        return (await response.Content.ReadFromJsonAsync<ProfileDataResponse>(cancellationToken), null, null, null);
    }
}
