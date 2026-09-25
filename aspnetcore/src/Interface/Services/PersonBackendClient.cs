using System.Net.Http.Json;
using ResearchFi.PersonPublicApi;

namespace CSC.PublicApi.Interface.Services;

public class PersonBackendClient : IPersonBackendClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PersonBackendClient> _logger;

    public PersonBackendClient(HttpClient httpClient, ILogger<PersonBackendClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProfileDataResponse?> SearchPersonsAsync(ProfileDataRequest request, string? clientId, CancellationToken cancellationToken = default)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/publicapi/profile")
        {
            Content = JsonContent.Create(request)
        };
        if (clientId != null)
        {
            httpRequest.Headers.Add("public-api-clientid", clientId);
        }
        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Person backend request failed with status code {StatusCode}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ProfileDataResponse>(cancellationToken);
    }
}
