using Api.DataAccess.Repositories;

namespace Api.Configuration
{
    public static class DataAccessExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFundingCallRepository, FundingCallRepository>();
            services.AddScoped<IFundingDecisionRepository, FundingDecisionRepository>();

            // Register every IIndexRepository<T> to IoC container.
            // Repositories are registered as IIndexRepository<T> and IIndexRepository,
            // latter signature allows the injection of collection of repositories, IEnumerable<IIndexRepository>.
            services.Scan(scan => scan
                .FromAssemblyOf<Program>()
                .AddClasses(classes => classes.AssignableTo(typeof(IIndexRepository)))
                .UsingRegistrationStrategy(Scrutor.RegistrationStrategy.Append)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                );

        }
    }
}
