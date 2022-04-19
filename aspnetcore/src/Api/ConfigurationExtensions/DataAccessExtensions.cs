using Api.DataAccess.Repositories;
using Api.Models.FundingCall;
using Api.Models.FundingDecision;

namespace Api.ConfigurationExtensions
{
    public static class DataAccessExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFundingCallRepository, FundingCallRepository>();
            services.AddScoped<IFundingDecisionRepository, FundingDecisionRepository>();
            services.AddScoped<IIndexRepository<FundingDecision>, FundingDecisionIndexRepository>();
            services.AddScoped<IIndexRepository<FundingCall>, FundingCallIndexRepository>();
            services.AddScoped<IIndexRepository<Models.Infrastructure.Infrastructure>, InfrastructureIndexRepository>();
        }
    }
}
