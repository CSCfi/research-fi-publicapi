using CSC.PublicApi.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CSC.PublicApi.Tests;

/// <summary>
/// Mocked authentication for integration tests.
/// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Add claims for the test user here.
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Test user"),
        };

        // Add every policy for the test user.
        var everyPolicyRoleName = ApiPolicies.PolicyRoleMap.Select(x => x.Value);
        var roleClaims = everyPolicyRoleName.Select(roleName => new Claim(ClaimTypes.Role, roleName));
        claims.AddRange(roleClaims);

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}