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

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        services.AddSettings(configuration);
        services.AddElasticSearch(configuration);
        services.AddAutoMapper(typeof(ApiPolicies).Assembly);
        services.AddTransient<IndexNameSettings>();
        services.AddTransient<FundingDecisionExporter>();
    })
    .Build();

var fundingDecisionExporter = host.Services.GetRequiredService<FundingDecisionExporter>();
fundingDecisionExporter.Export(jsonSerializerOptions);
