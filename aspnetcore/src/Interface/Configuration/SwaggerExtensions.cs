using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace CSC.PublicApi.Interface.Configuration;

public static class SwaggerExtensions
{
    public static void AddSwaggerAndApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = false;
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setupAction =>
        {
            setupAction.GroupNameFormat = "'v'V";
            setupAction.SubstituteApiVersionInUrl = true;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Needed because our models have non-unique names in different namespaces,
            // causing error "SchemaId already used for different type".
            options.CustomSchemaIds(type => type.FullName);
        });
        services.ConfigureOptions<SwaggerConfiguration>();
    }

    /// <summary>
    /// Converts type's full name to contain only parent namespace and the type name in the Swagger schema. 
    /// </summary>
    /// <param name="type"></param>
    /// <example>
    /// Api.Models.FundingDecision.FunderOrganization
    ///          ->
    ///      FundingDecision.FunderOrganization
    /// </example>
    /// <returns></returns>
    private static string GetShortTypeName(Type type)
    {
        // "Api.Models.FundingDecision.FunderOrganization"
        var fullTypeName = type.ToString();

        // [ "Api", "Models", "FundingDecision", "FunderOrganization" ]
        var typeNameSplitted = fullTypeName.Split('.');

        // [ "FundingDecision", "FunderOrganization" ]
        var typeNameAndDirectParent = typeNameSplitted[2..];

        // "FundingDecision.FunderOrganization"
        var shortTypeName = string.Join('.', typeNameAndDirectParent);

        return shortTypeName;
    }

    public static void UseSwaggerAndSwaggerUI(this WebApplication app)
    {
        var openApiSettings = app.Services.GetService<OpenApiSettings>();
        
        var basePath = !string.IsNullOrEmpty(openApiSettings?.BasePath) ? openApiSettings.BasePath : string.Empty;
        
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                var httpReqScheme = !string.IsNullOrEmpty(openApiSettings?.HttpScheme) ? openApiSettings.HttpScheme : httpReq.Scheme;
                swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = $"{httpReqScheme}://{httpReq.Host.Value}{basePath}" } };
            });
        });
        app.UseSwaggerUI(options =>
        {
            foreach (var description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                options.ConfigObject.AdditionalItems.Add("syntaxHighlight", false); // disable to improve performance with large responses
            }
        });
    }
}