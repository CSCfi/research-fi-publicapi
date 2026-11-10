using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Services;
using CSC.PublicApi.Interface.Tests.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ResearchFi.ProfilePublicApi;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class ProfileEndpointContractTest
{
    [Theory]
    [InlineData(TestAuthenticationMode.Anonymous, false, 401)]
    [InlineData(TestAuthenticationMode.AuthenticatedWithoutRoles, false, 403)]
    [InlineData(TestAuthenticationMode.AuthenticatedWithRoles, false, 403)]
    [InlineData(TestAuthenticationMode.AuthenticatedWithRoles, true, 200)]
    public async Task PostProfile_RequiresProfileReadRole(TestAuthenticationMode mode, bool correctRole, int expected)
    {
        var calls = 0;
        using var factory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = mode,
            Roles = new[] { ApiPolicies.PolicyRoleMap[correctRole ? ApiPolicies.Profile.Read : ApiPolicies.FundingCall.Read] }
        };
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            services.AddHttpClient<IProfileBackendClient, ProfileBackendClient>().ConfigurePrimaryHttpMessageHandler(
                () => new ProfileBackendContractTest.Handler((_, _) =>
                {
                    calls++;
                    return Task.FromResult(ProfileBackendContractTest.Response(
                        HttpStatusCode.OK, ProfileBackendContractTest.SuccessJson, "application/json"));
                }))));
        using var client = host.CreateClient();

        using var response = await client.PostAsJsonAsync("/v1/profiles", new ProfileDataRequest());

        Assert.Equal((HttpStatusCode)expected, response.StatusCode);
        Assert.Equal(correctRole ? 1 : 0, calls);
        if (correctRole)
        {
            var payload = await response.Content.ReadFromJsonAsync<ProfileDataResponse>();
            Assert.NotNull(payload);
            Assert.True(payload!.ProfileFound);
            Assert.False(payload.ConsentActive);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("synthetic-client")]
    public async Task PostProfile_TypedClientForwardsConfiguredTokenBodyAndOptionalClientId(string? clientId)
    {
        var calls = 0;
        using var factory = AuthorizedFactory(clientId);
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            services.AddHttpClient<IProfileBackendClient, ProfileBackendClient>().ConfigurePrimaryHttpMessageHandler(
                () => new ProfileBackendContractTest.Handler(async (request, token) =>
                {
                    calls++;
                    Assert.Equal(HttpMethod.Post, request.Method);
                    Assert.Equal("https://profile-backend.test/api/publicapi/profile", request.RequestUri!.AbsoluteUri);
                    Assert.Equal(new[] { "test-only-public-api-token" }, request.Headers.GetValues("publicapitoken"));
                    Assert.Equal(clientId != null, request.Headers.Contains("public-api-clientid"));
                    if (clientId != null)
                        Assert.Equal(new[] { clientId }, request.Headers.GetValues("public-api-clientid"));
                    Assert.True(token.CanBeCanceled);
                    using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync(token));
                    using var expected = JsonDocument.Parse(JsonSerializer.Serialize(new ProfileDataRequest
                        { ControllerROR = "https://ror.test/synthetic-controller" },
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)));
                    Assert.True(JsonElement.DeepEquals(expected.RootElement, body.RootElement));
                    return ProfileBackendContractTest.Response(HttpStatusCode.OK,
                        ProfileBackendContractTest.SuccessJson, "application/json");
                }))));
        using var client = host.CreateClient();

        using var response = await client.PostAsJsonAsync("/v1/profiles",
            new ProfileDataRequest { ControllerROR = "https://ror.test/synthetic-controller" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, calls);
    }

    [Theory]
    [InlineData(400, "application/problem+json")]
    [InlineData(403, "text/plain")]
    [InlineData(429, "application/json")]
    [InlineData(503, "text/plain")]
    public async Task PostProfile_PreservesBackendStatusBodyAndContentType(int code, string mediaType)
    {
        const string body = """{"error":"Synthetic backend rejection"}""";
        using var factory = AuthorizedFactory(null);
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            services.AddHttpClient<IProfileBackendClient, ProfileBackendClient>().ConfigurePrimaryHttpMessageHandler(
                () => new ProfileBackendContractTest.Handler((_, _) => Task.FromResult(
                    ProfileBackendContractTest.Response((HttpStatusCode)code, body, mediaType))))));
        using var client = host.CreateClient();

        using var response = await client.PostAsJsonAsync("/v1/profiles", new ProfileDataRequest());

        Assert.Equal((HttpStatusCode)code, response.StatusCode);
        Assert.Equal(body, await response.Content.ReadAsStringAsync());
        Assert.Equal($"{mediaType}; charset=utf-8", response.Content.Headers.ContentType!.ToString());
    }

    [Theory]
    [InlineData("", "application/json", 400)]
    [InlineData("{broken", "application/json", 400)]
    [InlineData("{}", "text/plain", 415)]
    public async Task PostProfile_InvalidBodyDoesNotCallBackend(string body, string mediaType, int status)
    {
        using var factory = AuthorizedFactory(null);
        using var client = factory.CreateClient();
        using var content = new StringContent(body, Encoding.UTF8, mediaType);

        using var response = await client.PostAsync("/v1/profiles", content);

        Assert.Equal((HttpStatusCode)status, response.StatusCode);
        Assert.Empty(factory.ProfileBackendRequests);
    }

    [Fact]
    public async Task PostProfile_NullBackendSuccessCurrentlyBecomesNoContent()
    {
        // P4-PROFILE: a missing backend contract silently becomes a successful 204.
        using var factory = AuthorizedFactory(null);
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            services.AddHttpClient<IProfileBackendClient, ProfileBackendClient>().ConfigurePrimaryHttpMessageHandler(
                () => new ProfileBackendContractTest.Handler((_, _) => Task.FromResult(
                    ProfileBackendContractTest.Response(HttpStatusCode.OK, "null", "application/json"))))));
        using var client = host.CreateClient();

        using var response = await client.PostAsJsonAsync("/v1/profiles", new ProfileDataRequest());

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("", await response.Content.ReadAsStringAsync());
    }

    private static TestWebApplicationFactory<Program> AuthorizedFactory(string? clientId) => new()
    {
        AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
        Roles = new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.Profile.Read] },
        Claims = clientId == null ? new Dictionary<string, string>() :
            new Dictionary<string, string> { ["clientId"] = clientId }
    };
}
