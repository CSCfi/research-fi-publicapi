using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Api.ConfigurationExtensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerAndApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'V";
                setupAction.SubstituteApiVersionInUrl = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureOptions<SwaggerConfiguration>();
        }

        public static void UseSwaggerAndSwaggerUI(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
