using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace CSC.PublicApi.Interface.Configuration;

public static class AuthExtensions
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration.GetSection("keycloak")["authority"];
        var audience = configuration.GetSection("keycloak")["audience"];
        var metadataAddress = configuration.GetSection("keycloak")["metadataaddress"];
        if (!bool.TryParse(configuration.GetSection("keycloak")["requirehttpsmetadata"], out var requireHttpsMetadata))
        {
            requireHttpsMetadata = true;
        }

        // Configure and add Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                
                options.Authority = authority;
                options.Audience = audience;
                options.MetadataAddress = metadataAddress;
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = authority,
                    ValidAudiences = new[] { audience },
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = _ => Task.CompletedTask,
                    OnForbidden = _ => Task.CompletedTask
                };
            });

        services.AddAuthorization(options =>
        {
            // Configure Policies
            foreach (var policyAndRole in ApiPolicies.PolicyRoleMap)
            {
                options.AddPolicy(policyAndRole.Key, policy => policy.RequireClaim(ClaimTypes.Role, policyAndRole.Value));
            }

        });
    }
}