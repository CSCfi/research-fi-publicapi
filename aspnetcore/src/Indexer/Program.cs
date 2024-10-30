using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Indexer.Configuration;
using CSC.PublicApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CSC.PublicApi.Indexer;

public class Program
{
    private const int DefaultQueryTimeout = 300;

    public static async Task Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        // Create and configure the host to support dependency injection, configuration, etc.
        var consoleHost = CreateHostBuilder(args).Build();

        // // Check if the database is ready for indexing.
        // var databasePreflightCheck = consoleHost.Services.GetRequiredService<DatabasePreflightCheck>();
        // if (!databasePreflightCheck.IsGood())
        // {
        //     return;
        // }

        // Get the "Main" service which handles the indexing.
        var indexer = consoleHost.Services.GetRequiredService<Indexer>();

        // Start indexing.
        await indexer.Start();

        // Flush logs
        Log.CloseAndFlush();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices((hostContext, services) =>
        {
            // Register the "Main" service.
            services.AddTransient<Indexer>();

            // Register the database checker service.
            services.AddTransient<DatabasePreflightCheck>();

            // Register settings.
            services.AddSettings(hostContext.Configuration);

            // Add ElasticSearch.
            services.AddElasticSearch(hostContext.Configuration);

            // Add ElasticSearchIndexingService.
            services.AddScoped<IElasticSearchIndexService, ElasticSearchIndexService>();

            services.AddMemoryCache();

            if (!int.TryParse(hostContext.Configuration["QueryTimeout"], out var queryTimeout))
            {
                queryTimeout = DefaultQueryTimeout;
            }

            // Configure db & entity framework.
            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlServer("name=dbconnectionstring", opt =>
                {
                    opt.CommandTimeout(queryTimeout);
                });
                options.ConfigureWarnings(x => x.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });

            services.AddRepositories();

            services.AddAutoMapper(typeof(IUnitOfWork).Assembly);
        })
        .ConfigureHostConfiguration(configurationBuilder => configurationBuilder
            // Most of the configuration comes from environment variables.
            .AddEnvironmentVariables()
            // For local dev we get configuration from user secrets.
            .AddUserSecrets(typeof(Program).Assembly, true)
            .Build());
}