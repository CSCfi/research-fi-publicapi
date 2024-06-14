using BulkExport;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface;
using CSC.PublicApi.Interface.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
fundingDecisionExporter.Export();
