using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Http;

namespace CSC.PublicApi.Logging;

/// <summary>
/// Http client to support basic authenticated when using Serilog.Sinks.Http.
/// </summary>
public class BasicAuthenticationHttpClient : IHttpClient
{
    private readonly HttpClient _httpClient;

    public BasicAuthenticationHttpClient()
    {
        _httpClient = new HttpClient();
    }

    public void Configure(IConfiguration configuration)
    {
        var username = configuration["logStash:username"];
        var password = configuration["logStash:password"];

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream contentStream)
    {
        using var content = new StreamContent(contentStream);
        content.Headers.Add("Content-Type", "application/json");

        var response = await _httpClient
            .PostAsync(requestUri, content)
            .ConfigureAwait(false);

        return response;
    }

    public void Dispose() => _httpClient.Dispose();
}