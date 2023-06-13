using System.Diagnostics;
using CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace CSC.PublicApi.ElasticService;

public static class ElasticSearchExtensions
{
    public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the main search service
        services.AddScoped(typeof(ISearchService<,>), typeof(ElasticSearchService<,>));

        // Register query generator for each model
        services.Scan(scan => scan
            .FromAssemblyOf<SearchResult>()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryGenerator<,>)))
            .UsingRegistrationStrategy(Scrutor.RegistrationStrategy.Throw)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        // Get settings for the ElasticSearch client
        var connectionSettings = GetConnectionSettings(configuration);

        // Instantiate the client with the settings
        var elasticClient = new ElasticClient(connectionSettings);

        // Register the client as singleton
        services.AddSingleton<IElasticClient>(elasticClient);
    }

    private static ConnectionSettings GetConnectionSettings(IConfiguration configuration)
    {
        var elasticSearchClusterUrl = configuration["ElasticSearch:Url"] ?? throw new InvalidOperationException("ElasticSearch url missing.");

        var connectionSettings = new ConnectionSettings(new Uri(elasticSearchClusterUrl))
            .DefaultFieldNameInferrer(i => i); // This forces elastic to store .Net objects using type names, instead of camel casing. This enables using nameof when referring to fields.

        if (Debugger.IsAttached)
        {
            // Disable direct streaming when debugging to be able to log the DebugInformation in the ES response.
            connectionSettings.DisableDirectStreaming();
        }

        var elasticSearchUserName = configuration["ElasticSearch:Username"];
        var elasticSearchPassword = configuration["ElasticSearch:Password"];

        // If both are missing, basic auth is not used.
        if (string.IsNullOrWhiteSpace(elasticSearchUserName) && string.IsNullOrWhiteSpace(elasticSearchPassword))
        {
            return connectionSettings;
        }

        if (string.IsNullOrWhiteSpace(elasticSearchUserName))
        {
            throw new InvalidOperationException("ElasticSearch username missing.");
        }

        if (string.IsNullOrWhiteSpace(elasticSearchPassword))
        {
            throw new InvalidOperationException("ElasticSearch password missing.");
        }

        return connectionSettings.BasicAuthentication(elasticSearchUserName, elasticSearchPassword);
    }
}