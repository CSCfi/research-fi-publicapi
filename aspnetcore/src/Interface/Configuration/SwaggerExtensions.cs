using System.Text.RegularExpressions;
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
            /*
             * Modify generated schema IDs.
             * Remove version number, which are used in namespace to separate API versions, from the schema ID.
             * Example: ResearchFi.VirtaJtp.V1.DuplikaatitAPI -> ResearchFi.VirtaJtp.DuplikaatitAPI
             */
            options.CustomSchemaIds(t =>
            {
                var full = t.FullName ?? t.Name;
                full = Regex.Replace(full, @"(\.|^)V\d+(_\d+)?\.", "$1"); // removes ".V1." or ".V1_1." etc.
                return full;
            });
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
            options.ConfigObject.AdditionalItems.Add("syntaxHighlight", false); // disable to improve performance with large responses
            foreach (var description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
            {
                // At the moment, API version V2 is an example and should not be visible in the API documentation.
                if (description.GroupName.StartsWith("v2", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
    }
}