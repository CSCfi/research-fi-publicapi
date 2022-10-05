using System.Text.Json;
//using Api.DataAccess;
//using Api.DatabaseContext;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface;
using CSC.PublicApi.Interface.Configuration;
using CSC.PublicApi.Interface.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register settings.
builder.Services.AddSettings(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

// Configure and add Swagger with api versioning.
builder.Services.AddSwaggerAndApiVersioning();

// Configure and add ElasticSearch.
builder.Services.AddElasticSearch(builder.Configuration);

builder.Services.Scan(scan =>
    scan.FromCallingAssembly()
        .AddClasses()
        .AsMatchingInterface()
        .WithScopedLifetime());

// Configure authentication and authorization.
builder.Services.AddAuth(builder.Configuration);

// Configure db & entity framework
/*builder.Services.AddDbContext<ApiDbContext>(options =>
{
    // Get connection string named 'dbconnectionstring' from environment variables, appsettings or user secrets.
    options.UseSqlServer("name=dbconnectionstring");
    // Throw when EF's generated sql query might be slow.
    options.ConfigureWarnings(x => x.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRepositories();*/

// Register Automapper and maps
builder.Services.AddAutoMapper(typeof(ApiPolicies).Assembly);

builder.Services.AddHttpLogging(options =>
{
    options.ResponseHeaders.Add(CorrelationIdMiddleware.CorrelationIdHeaderName);
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

// Generate correlation ids for requests.
app.UseMiddleware<CorrelationIdMiddleware>();

// Error handler to prevent exceptions details showing up for end users.
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.UseSwaggerAndSwaggerUI();

app.MapControllers();

app.Run();

// Needed for test project access to the configuration.
namespace CSC.PublicApi.Interface
{
    public partial class Program { }
}