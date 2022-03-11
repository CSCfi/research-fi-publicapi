using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Api.ConfigurationExtensions
{
    public static class AuthExtensions
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure and add Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration.GetSection("keycloak")["authority"];
                    options.Audience = configuration.GetSection("keycloak")["audience"];
                    options.MetadataAddress = configuration.GetSection("keycloak")["metadataaddress"];
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration.GetSection("keycloak")["authority"],
                        ValidAudiences = new[] { configuration.GetSection("keycloak")["audience"] },
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            return Task.CompletedTask;
                        }
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
}
