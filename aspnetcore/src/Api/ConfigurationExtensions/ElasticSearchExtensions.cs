using Nest;

namespace Api.ConfigurationExtensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchClusterUrl = configuration["ELASTICSEARCH_URL"] ?? throw new InvalidOperationException("ElasticSearch url missing.");
            var connectionSettings = new ConnectionSettings(new Uri(elasticSearchClusterUrl));
            var elasticClient = new ElasticClient(connectionSettings);
            services.AddSingleton<IElasticClient>(elasticClient);
        }
    }
}
