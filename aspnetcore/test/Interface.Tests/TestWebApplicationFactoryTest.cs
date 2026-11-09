using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Services;
using CSC.PublicApi.Interface.Services.V1;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class TestWebApplicationFactoryTest
{
    [Fact]
    public async Task FundingCallEndpoint_UsesConfiguredAuthenticationModeAndRequiredRole()
    {
        using var anonymousFactory = new TestWebApplicationFactory<Program>();
        using var unauthenticatedClient = anonymousFactory.CreateClient();
        using var authenticatedFactory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = TestAuthenticationMode.AuthenticatedWithoutRoles
        };
        using var authenticatedClient = authenticatedFactory.CreateClient();
        using var authorizedFactory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
            Roles = new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.FundingCall.Read] },
            Claims = new Dictionary<string, string> { ["clientId"] = "synthetic-client" }
        };
        using var authorizedClient = authorizedFactory.CreateClient();

        var responses = await Task.WhenAll(
            unauthenticatedClient.GetAsync("/v1/funding-calls"),
            authenticatedClient.GetAsync("/v1/funding-calls"),
            authorizedClient.GetAsync("/v1/funding-calls"));

        using (responses[0])
        using (responses[1])
        using (responses[2])
        {
            responses[0].StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            responses[1].StatusCode.Should().Be(HttpStatusCode.Forbidden);
            responses[2].StatusCode.Should().Be(HttpStatusCode.OK);
            (await responses[2].Content.ReadAsStringAsync()).Should().Be("[]");
            responses[2].Headers.GetValues("x-current-page").Should().ContainSingle().Which.Should().Be("1");
        }
    }

    [Fact]
    public async Task TestHost_ResolvesScrutorServicesAndUsesIsolatedTypedProfileClient()
    {
        using var factory = new TestWebApplicationFactory<Program>();

        factory.Services.GetRequiredService<IFundingCallService>()
            .Should().BeOfType<FundingCallService>();
        var profileBackendClient = factory.Services.GetRequiredService<IProfileBackendClient>()
            .Should().BeOfType<ProfileBackendClient>()
            .Subject;

        var (_, errorStatusCode, _, _) =
            await profileBackendClient.SearchProfilesAsync(null!, "synthetic-client");

        errorStatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        factory.ProfileBackendRequests.Should().ContainSingle(request =>
            request.RequestUri != null &&
            request.RequestUri.Host == "profile-backend.test" &&
            request.PublicApiToken == "test-only-public-api-token");
    }
}
