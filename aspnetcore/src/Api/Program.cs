using Api.ConfigurationExtensions;
using Api.DataAccess;
using Api.DatabaseContext;
using Api.Models;
using Api.Models.FundingCall;
using Api.Models.FundingDecision;
using Api.Services;
using Api.Services.ElasticSearchQueryGenerators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerAndApiVersioning();

builder.Services.AddScoped(typeof(ISearchService<,>), typeof(ElasticSearchService<,>));
builder.Services.AddScoped<IQueryGenerator<PublicationSearchParameters, Publication>, PublicationQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<FundingCallSearchParameters, FundingCall>, FundingCallQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<FundingDecisionSearchParameters, FundingDecision>, FundingDecisionQueryGenerator>();

// Configure and add ElasticSearch
builder.Services.AddElasticSearch(builder.Configuration);

// Configure authentication and authorization.
builder.Services.AddAuth(builder.Configuration);

// Configure db & entity framework
builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlServer("name=dbconnectionstring"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRepositories();

var app = builder.Build();

app.UseSwaggerAndSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Needed for test project access to the configuration.
public partial class Program { }