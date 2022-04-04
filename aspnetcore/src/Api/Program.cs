using Api.ConfigurationExtensions;
using Api.DataAccess;
using Api.DatabaseContext;
using Api.Maps;
using Api.Middleware;
using Api.Models;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Api.Models.FundingDecision;
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
builder.Services.AddScoped<IQueryGenerator<PublicationSearchParameters, Api.Models.Publication>, PublicationQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<FundingCallSearchParameters, FundingCall>, FundingCallQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<FundingDecisionSearchParameters, FundingDecision>, FundingDecisionQueryGenerator>();

// Configure and add ElasticSearch.
builder.Services.AddElasticSearch(builder.Configuration);

// Configure authentication and authorization.
builder.Services.AddAuth(builder.Configuration);

// Configure db & entity framework
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer("name=dbconnectionstring");
    options.ConfigureWarnings(x => x.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRepositories();

// Register Automapper and maps
builder.Services.AddAutoMapper(typeof(Api.ApiPolicies));
builder.Services.AddScoped<IMapper<DimCallProgramme, FundingCall>, FundingCallEntityToApiModel>();

var app = builder.Build();
// Generate correlation ids for requests.
app.UseMiddleware<CorrelationIdMiddleware>();
// Error handler
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.UseSwaggerAndSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Needed for test project access to the configuration.
public partial class Program { }