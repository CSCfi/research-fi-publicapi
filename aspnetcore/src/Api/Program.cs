using Api.ConfigurationExtensions;
using Api.Models;
using Api.Models.FundingCall;
using Api.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddScoped(typeof(ISearchService<,>), typeof(ElasticSearchService<,>));
builder.Services.AddScoped<IQueryGenerator<PublicationSearchParameters, Publication>, PublicationQueryGenerator>();
builder.Services.AddScoped<IQueryGenerator<FundingCallSearchParameters, FundingCall>, FundingCallQueryGenerator>();

// Configure and add ElasticSearch
builder.Services.AddElasticSearch(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
