using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using BulkExport;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface;
using CSC.PublicApi.Interface.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/*
 * This application exports all documents from Elasticsearch index into json files. Data is converted from Elasticsearch model to API model.
 * The purpose of this application is to enable bulk export of all data. Application is not intended to be executed automatically in production.
 * Instead, it should be considered as a tool, which a developer can use when a full data dump is needed in json file format.
 *
 * The application uses the same configuration as Indexer and Interface applications, except that it requires an added parameter:
 *   EXPORTER:BASEDIRECTORY - sets the base directory where the json files are written. It must be defined without trailing slash, for example, "/tmp"
 */

public class Program
{
    private const int DefaultQueryTimeout = 300;
    public static async Task Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{environment}.json", true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();
        var consoleHost = CreateHostBuilder(args).Build();

        // Define json serializer options
        // https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=net-6.0
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/character-encoding
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties?pivots=dotnet-6-0
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/ignore-properties#ignore-all-null-value-properties
        var jsonSerializerOptions = new JsonSerializerOptions {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        // Start export
        var exporter = consoleHost.Services.GetRequiredService<Exporter>();
        exporter.Export(jsonSerializerOptions);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddTransient<Exporter>();
            services.AddSettings(hostContext.Configuration);
            services.AddElasticSearch(hostContext.Configuration);
            services.AddAutoMapper(typeof(ApiPolicies).Assembly);
            services.AddScoped<IElasticSearchIndexService, ElasticSearchIndexService>();
        })
        .ConfigureHostConfiguration(configurationBuilder => configurationBuilder
            .AddEnvironmentVariables()
            .AddUserSecrets(typeof(Program).Assembly, true)
            .Build());
}