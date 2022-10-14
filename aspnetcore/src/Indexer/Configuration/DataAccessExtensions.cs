using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CSC.PublicApi.Indexer.Configuration;

public static class DataAccessExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        // Register every IIndexRepository<T> to IoC container.
        // Repositories are registered as IIndexRepository<T> and IIndexRepository,
        // latter signature allows the injection of collection of repositories, IEnumerable<IIndexRepository>.
        services.Scan(scan => scan
            .FromAssemblyOf<IUnitOfWork>()
            .AddClasses(classes => classes.AssignableTo(typeof(IIndexRepository)))
            .UsingRegistrationStrategy(Scrutor.RegistrationStrategy.Append)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

    }
}