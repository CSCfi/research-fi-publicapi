using Api.Services;
using Api.Services.ElasticSearchQueryGenerators;
using Nest;

namespace Api.Configuration
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the main search service
            services.AddScoped(typeof(ISearchService<,>), typeof(ElasticSearchService<,>));

            // Register query generator for each model
            services.Scan(scan => scan
                .FromAssemblyOf<Program>()
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
