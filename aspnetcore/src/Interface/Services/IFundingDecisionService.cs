using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.FundingDecision;

namespace CSC.PublicApi.Interface.Services;

public interface IFundingDecisionService
{
    Task<(IEnumerable<FundingDecision>, SearchResult)> GetFundingDecisions(GetFundingDecisionQueryParameters queryParameters);
}