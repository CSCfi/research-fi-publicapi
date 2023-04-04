using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace CSC.PublicApi.Interface.Configuration;

public class SwaggerConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IConfiguration _configuration;
    private readonly OpenApiSettings _openApiSettings;

    public SwaggerConfiguration(
        IApiVersionDescriptionProvider apiVersionProvider,
        IConfiguration configuration,
        OpenApiSettings openApiSettings)
    {
        _provider = apiVersionProvider;
        _configuration = configuration;
        _openApiSettings = openApiSettings;
    }

    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerGenOptions options)
    {
        // Needed for getting Swagger UI page's controller/model member descriptions from code comments.
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"ApiModels.xml"));

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
                Title = _openApiSettings.Title,
                Version = apiVersionDescription.ApiVersion.ToString(),
                Description = _openApiSettings.Description,
                Contact = new OpenApiContact
                {
                    Name = _openApiSettings.ContactName,
                    Email = _openApiSettings.ContactEmail,
                    Url = _openApiSettings.ContactUrl
                },
                TermsOfService = _openApiSettings.TermsOfService
            };

            if (apiVersionDescription.IsDeprecated)
            {
                openApiInfo.Description += " (Deprecated)";
            }

            options.SwaggerDoc(apiVersionDescription.GroupName, openApiInfo);
        }
    }
}