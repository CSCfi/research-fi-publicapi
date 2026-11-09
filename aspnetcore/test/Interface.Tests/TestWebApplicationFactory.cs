using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Services;
using CSC.PublicApi.Interface.Tests.TestHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using ResearchFi.ProfilePublicApi;

namespace CSC.PublicApi.Interface.Tests;

public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly List<ProfileBackendRequest> _profileBackendRequests = new();

    public TestAuthenticationMode AuthenticationMode { get; set; } = TestAuthenticationMode.Anonymous;

    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();

    public IReadOnlyDictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();

    public IReadOnlyList<ProfileBackendRequest> ProfileBackendRequests => _profileBackendRequests;

    public bool UseRealElasticsearch { get; set; }

    public string? ElasticsearchUrl { get; set; }

    public IReadOnlyDictionary<string, string> IndexNames { get; set; } = new Dictionary<string, string>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var elasticsearchUrl = UseRealElasticsearch
            ? ElasticsearchUrl ?? throw new InvalidOperationException(
                "An Elasticsearch URL is required when using the real Elasticsearch client.")
            : "http://127.0.0.1:1";

        builder.UseContentRoot(".");
        builder.UseEnvironment("Test");
        builder.UseSetting("ElasticSearch:Url", elasticsearchUrl);
        builder.UseSetting("Keycloak:audience", "test-audience");
        builder.UseSetting("Keycloak:authority", "http://keycloak.test/realms/test-realm");
        builder.UseSetting(
            "Keycloak:metadataaddress",
            "http://keycloak.test/realms/test-realm/.well-known/openid-configuration");
        builder.UseSetting("Keycloak:requirehttpsmetadata", "false");
        builder.UseSetting("ProfileBackend:BaseUrl", "https://profile-backend.test/");
        builder.UseSetting("ProfileBackend:PublicApiToken", "test-only-public-api-token");
        foreach (var (modelType, indexName) in IndexNames)
        {
            builder.UseSetting($"IndexNames:{modelType}", indexName);
        }

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ElasticSearch:Url"] = elasticsearchUrl,
                ["Keycloak:audience"] = "test-audience",
                ["Keycloak:authority"] = "http://keycloak.test/realms/test-realm",
                ["Keycloak:metadataaddress"] =
                    "http://keycloak.test/realms/test-realm/.well-known/openid-configuration",
                ["Keycloak:requirehttpsmetadata"] = "false",
                ["ProfileBackend:BaseUrl"] = "https://profile-backend.test/",
                ["ProfileBackend:PublicApiToken"] = "test-only-public-api-token",
                ["OpenApiSettings:Title"] = "Public API tests",
                ["OpenApiSettings:Description"] = "Synthetic test configuration.",
                ["Serilog:Using:0"] = "Serilog.Sinks.Console",
                ["Serilog:MinimumLevel:Default"] = "Warning",
                ["Serilog:WriteTo:0:Name"] = "Console",
                ["Serilog:WriteTo:0:Args:outputTemplate"] = "{Message}{NewLine}{Exception}"
            };
            foreach (var (modelType, indexName) in IndexNames)
            {
                settings[$"IndexNames:{modelType}"] = indexName;
            }

            configuration.AddInMemoryCollection(settings);
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>("Test", options =>
                {
                    options.Mode = AuthenticationMode;
                    options.Roles = Roles.ToArray();
                    options.Claims = new Dictionary<string, string>(Claims);
                });

            if (!UseRealElasticsearch)
            {
                services.RemoveAll(typeof(ISearchService<,>));
                services.AddScoped(typeof(ISearchService<,>), typeof(EmptySearchService<,>));
                services.RemoveAll<IElasticClient>();
                services.AddSingleton<IElasticClient>(_ => ElasticTestClient.Create(_ => { }));
            }

            services.AddHttpClient<IProfileBackendClient, ProfileBackendClient>()
                .ConfigurePrimaryHttpMessageHandler(
                    () => new RecordingProfileBackendHandler(_profileBackendRequests));
        });
    }

    private sealed class RecordingProfileBackendHandler(
        ICollection<ProfileBackendRequest> requests) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = request.Headers.TryGetValues("publicapitoken", out var values)
                ? values.SingleOrDefault()
                : null;
            requests.Add(new ProfileBackendRequest(request.RequestUri, token));

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = new StringContent(
                    "Profile backend access is disabled in tests.",
                    Encoding.UTF8,
                    "text/plain")
            });
        }
    }
}

public sealed record ProfileBackendRequest(Uri? RequestUri, string? PublicApiToken);

internal sealed class EmptySearchService<TIn, TOut> : ISearchService<TIn, TOut>
    where TOut : class
{
    public Task<(IEnumerable<TOut>, SearchResult)> Search(TIn searchParameters, int pageNumber, int pageSize)
    {
        return Task.FromResult(((IEnumerable<TOut>)Array.Empty<TOut>(), new SearchResult(pageNumber, pageSize, 0)));
    }

    public Task<(IEnumerable<TOut>, SearchAfterResult)> SearchAfter(
        TIn searchParameters,
        int pageSize,
        long? searchAfter)
    {
        throw new InvalidOperationException("Search-after is not configured in the test host.");
    }

    public Task<TOut?> GetSingle(string id)
    {
        throw new InvalidOperationException("Single-document search is not configured in the test host.");
    }
}
