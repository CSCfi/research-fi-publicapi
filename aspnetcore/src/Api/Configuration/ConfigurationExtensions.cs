using Microsoft.Extensions.Options;

namespace Api.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // Register ElasticSearch index name settings
            var mySettings = new IndexNameSettings();
            new ConfigureFromConfigurationOptions<IndexNameSettings>(configuration.GetSection("IndexNames")).Configure(mySettings);
            services.AddSingleton(mySettings);
        }
    }
}
