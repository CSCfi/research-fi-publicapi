using Nest;

namespace Api.ConfigurationExtensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            // Get settings for the ElasticSearch client
            var connectionSettings = GetConnectionSettings(configuration);

            // Instantiate the client with the settings
            var elasticClient = new ElasticClient(connectionSettings);
            
            // Register the client as singleton
            services.AddSingleton<IElasticClient>(elasticClient);
        }

        private static ConnectionSettings GetConnectionSettings(IConfiguration configuration)
        {
            var elasticSearchClusterUrl = configuration["ELASTICSEARCH:URL"] ?? throw new InvalidOperationException("ElasticSearch url missing.");

            var connectionSettings = new ConnectionSettings(new Uri(elasticSearchClusterUrl));

            if (configuration["ELASTICSEARCH:USERNAME"] == null && configuration["ELASTICSEARCH:PASSWORD"] == null)
            {
                return connectionSettings;
            }

            var elasticSearchUserName = configuration["ELASTICSEARCH:USERNAME"] ?? throw new InvalidOperationException("ElasticSearch username missing.");
            var elasticSearchPassword = configuration["ELASTICSEARCH:PASSWORD"] ?? throw new InvalidOperationException("ElasticSearch password missing.");

            return connectionSettings.BasicAuthentication(elasticSearchUserName, elasticSearchPassword);
        }
    }
}
