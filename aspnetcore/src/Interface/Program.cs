using System.Text.Json;
using System.Text.Json.Serialization;
//using Api.DataAccess;
//using Api.DatabaseContext;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface;
using CSC.PublicApi.Interface.Configuration;
using CSC.PublicApi.Interface.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Register settings.
builder.Services.AddSettings(builder.Configuration);

// Configure logging with Serilog
builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

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
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHeaderPropagation(options => options.Headers.Add("x-correlation-id"));

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

var app = builder.Build();

app.UseHeaderPropagation();

// Generate correlation ids for requests.
app.UseMiddleware<CorrelationIdMiddleware>();

// Add properties from the HTTP request to the logging.
app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = (context, httpContext) =>
{
    var correlationId = httpContext.Items[CorrelationIdMiddleware.CorrelationIdHeaderName];
    var clientId = httpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
    var organizationId = httpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "organizationid")?.Value;
    
    context.Set("correlationId", correlationId);
    context.Set("clientId", clientId);
    context.Set("organizationId", organizationId);
});

app.UseAuthentication();
app.UseAuthorization();

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