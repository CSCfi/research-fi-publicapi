using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Api.Configuration
{
    public class SwaggerConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _configuration;

        public SwaggerConfiguration(
            IApiVersionDescriptionProvider apiVersionProvider,
            IConfiguration configuration)
        {
            _provider = apiVersionProvider;
            _configuration = configuration;
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerGenOptions options)
        {
            // Needed for getting Swagger UI page's controller/model member descriptions from code comments.
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

            // Setup OAuth login for Swagger UI
            var authorityUrl = _configuration.GetSection("keycloak")["authority"] ?? throw new InvalidOperationException("Could not get authority url from configuration.");
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Scheme = "Bearer",
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{authorityUrl}/protocol/openid-connect/token")
                    }
                }
            };

            options.AddSecurityDefinition(name: "Bearer", securityScheme: securityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });

            // Version information for Swagger UI
            foreach (var apiVersionDescription in _provider.ApiVersionDescriptions)
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "API",
                    Version = apiVersionDescription.ApiVersion.ToString(),
                };

                if (apiVersionDescription.IsDeprecated)
                {
                    openApiInfo.Description += " (Deprecated)";

                }

                options.SwaggerDoc(apiVersionDescription.GroupName, openApiInfo);
            }
        }
    }
}
