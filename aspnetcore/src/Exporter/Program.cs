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

        // Create and configure the host to support dependency injection, configuration, etc.
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
            // Most of the configuration comes from environment variables.
            .AddEnvironmentVariables()
            // For local dev we get configuration from user secrets.
            .AddUserSecrets(typeof(Program).Assembly, true)
            .Build());
}