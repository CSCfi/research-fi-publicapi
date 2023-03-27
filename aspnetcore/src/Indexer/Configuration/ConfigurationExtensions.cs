using CSC.PublicApi.ElasticService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CSC.PublicApi.Indexer.Configuration;

public static class ConfigurationExtensions
{
    public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        // Register ElasticSearch index name settings
        var indexNameSettings = new IndexNameSettings();
        new ConfigureFromConfigurationOptions<IndexNameSettings>(configuration.GetSection("IndexNames")).Configure(indexNameSettings);
        services.AddSingleton(indexNameSettings);

        AddSettings<IndexNameSettings>("IndexNames", services, configuration);
    }

    private static void AddSettings<T>(string sectionName, IServiceCollection services, IConfiguration configuration) where T : class, new()
    {
        var settings = new T();
        new ConfigureFromConfigurationOptions<T>(configuration.GetSection(sectionName)).Configure(settings);
        services.AddSingleton(settings);
    }
}