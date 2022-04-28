using Api.ConfigurationExtensions;
using Api.DataAccess;
using Api.DatabaseContext;
using Api.Middleware;
using Api.Models.FundingCall;
using Api.Models.FundingDecision;
using Api.Models.Infrastructure;
using Api.Models.Publication;
using Api.Services;
using Api.Services.ElasticSearchQueryGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure and add Swagger with api versioning.
builder.Services.AddSwaggerAndApiVersioning();

// Register ElasticSearch query generators.
builder.Services.AddScoped(typeof(ISearchService<,>), typeof(ElasticSearchService<,>));
builder.Services.AddScoped<IQueryGenerator<PublicationSearchParameters, Api.Models.Publication.Publication>, PublicationQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<FundingCallSearchParameters, Api.Models.FundingCall.FundingCall>, FundingCallQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<FundingDecisionSearchParameters, FundingDecision>, FundingDecisionQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<InfrastructureSearchParameters, Api.Models.Infrastructure.Infrastructure>, InfrastructureQueryGenerator>();

// Configure and add ElasticSearch.
builder.Services.AddElasticSearch(builder.Configuration);

// Configure authentication and authorization.
builder.Services.AddAuth(builder.Configuration);

// Configure db & entity framework
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    // Get connection string named 'dbconnectionstring' from environment variables, appsettings or user secrets.
    options.UseSqlServer("name=dbconnectionstring");
    // Throw when EF's generated sql query might be slow.
    options.ConfigureWarnings(x => x.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRepositories();

// Register Automapper and maps
builder.Services.AddAutoMapper(typeof(Api.ApiPolicies));

builder.Services.AddHttpLogging(options =>
{
    options.ResponseHeaders.Add(CorrelationIdMiddleware.CorrelationIdHeaderName);
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

app.UseHttpLogging();

app.UseSwaggerAndSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// Generate correlation ids for requests.
app.UseMiddleware<CorrelationIdMiddleware>();

// Error handler to prevent exceptions details showing up for end users.
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.MapControllers();

app.Run();

// Needed for test project access to the configuration.
public partial class Program { }