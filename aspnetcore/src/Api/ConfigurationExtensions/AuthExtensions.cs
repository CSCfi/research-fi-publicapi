using Microsoft.AspNetCore.Authentication.JwtBearer;

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
                });

            services.AddAuthorization(options =>
            {

            });
        }
    }
}
