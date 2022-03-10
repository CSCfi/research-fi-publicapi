using Api.DataAccess.Repositories;

namespace Api.ConfigurationExtensions
{
    public static class DataAccessExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFundingCallRepository, FundingCallRepository>();
            services.AddScoped<IFundingDecisionRepository, FundingDecisionRepository>();
        }
    }
}
