using System.Net.Http.Json;
using ResearchFi.Query;

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

    public async Task<string?> SearchPersonsAsync(GetPersonsQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("persons/search", queryParameters, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Person backend request failed with status code {StatusCode}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
