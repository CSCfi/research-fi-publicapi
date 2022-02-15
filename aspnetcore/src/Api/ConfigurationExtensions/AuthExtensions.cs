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
                    options.Audience = configuration.GetSection("keycloak")["audience"];
                    options.Authority = configuration.GetSection("keycloak")["authority"];
                    options.MetadataAddress = configuration.GetSection("keycloak")["metadataaddress"];
                });

            services.AddAuthorization(options =>
            {

            });
        }
    }
}
