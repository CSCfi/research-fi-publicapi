using System.Net;
using System.Net.Http.Json;
using ResearchFi.ProfilePublicApi;

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

    public async Task<(ProfileDataResponse? Response, HttpStatusCode? ErrorStatusCode, string? ErrorContent, string? ErrorContentType)> SearchPersonsAsync(ProfileDataRequest request, string? clientId, CancellationToken cancellationToken = default)
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
            _logger.LogWarning("Person backend request failed with status code {StatusCode}", response.StatusCode);
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var errorContentType = response.Content.Headers.ContentType?.ToString();
            return (null, response.StatusCode, errorContent, errorContentType);
        }

        return (await response.Content.ReadFromJsonAsync<ProfileDataResponse>(cancellationToken), null, null, null);
    }
}
