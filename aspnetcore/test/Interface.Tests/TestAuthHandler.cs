using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSC.PublicApi.Interface.Tests;

public enum TestAuthenticationMode
{
    Anonymous,
    AuthenticatedWithoutRoles,
    AuthenticatedWithRoles
}

public sealed class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public TestAuthenticationMode Mode { get; set; }

    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();

    public IReadOnlyDictionary<string, string> Claims { get; set; } =
        new Dictionary<string, string>();
}

public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<TestAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Options.Mode == TestAuthenticationMode.Anonymous)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "Test user")
        };
        claims.AddRange(Options.Claims.Select(claim => new Claim(claim.Key, claim.Value)));
        claims.AddRange(Options.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
