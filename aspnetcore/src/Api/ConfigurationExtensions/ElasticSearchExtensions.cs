using Nest;

namespace Api.ConfigurationExtensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchClusterUrl = configuration["ELASTICSEARCH:URL"] ?? throw new InvalidOperationException("ElasticSearch url missing.");
            var elasticSearchUserName = configuration["ELASTICSEARCH:USERNAME"] ?? throw new InvalidOperationException("ElasticSearch username missing.");
            var elasticSearchPassword = configuration["ELASTICSEARCH:PASSWORD"] ?? throw new InvalidOperationException("ElasticSearch password missing.");
            
            var connectionSettings = new ConnectionSettings(new Uri(elasticSearchClusterUrl))
                .BasicAuthentication(elasticSearchUserName, elasticSearchPassword);
            
            var elasticClient = new ElasticClient(connectionSettings);
            
            services.AddSingleton<IElasticClient>(elasticClient);
        }
    }
}
