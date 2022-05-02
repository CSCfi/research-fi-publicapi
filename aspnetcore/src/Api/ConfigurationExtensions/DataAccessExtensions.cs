using Api.DataAccess.Repositories;

namespace Api.ConfigurationExtensions
{
    public static class DataAccessExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFundingCallRepository, FundingCallRepository>();
            services.AddScoped<IFundingDecisionRepository, FundingDecisionRepository>();

            // Register every IIndexRepository<T> to IoC container.
            services.Scan(scan => scan
                .FromAssemblyOf<Program>()
                .AddClasses(classes => classes.AssignableTo(typeof(IIndexRepository<>)))
                .UsingRegistrationStrategy(Scrutor.RegistrationStrategy.Throw)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                );

        }
    }
}
