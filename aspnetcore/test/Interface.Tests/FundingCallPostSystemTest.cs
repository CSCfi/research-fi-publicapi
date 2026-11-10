using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using FundingCall = ResearchFi.FundingCall.FundingCall;

namespace CSC.PublicApi.Interface.Tests;

public class FundingCallPostSystemTest
{
    [Theory]
    [InlineData(TestAuthenticationMode.Anonymous)]
    [InlineData(TestAuthenticationMode.AuthenticatedWithoutRoles)]
    [InlineData(TestAuthenticationMode.AuthenticatedWithRoles)]
    public async Task PostFundingCall_IsNotSupported(TestAuthenticationMode authenticationMode)
    {
        using var factory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = authenticationMode,
            Roles = new[]
            {
                ApiPolicies.PolicyRoleMap[ApiPolicies.FundingCall.Read],
                ApiPolicies.PolicyRoleMap[ApiPolicies.FundingCall.Write]
            }
        };
        using var client = factory.CreateClient();
        using var content = JsonContent.Create(new FundingCall { NameFi = "Synthetic unsupported write" });
        using var response = await client.PostAsync("v1/funding-calls", content);

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        Assert.Equal(new[] { "GET" }, response.Content.Headers.Allow);
    }
}
