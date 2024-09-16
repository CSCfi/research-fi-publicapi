using CSC.PublicApi.ElasticService;
using ResearchFi.FundingDecision;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IFundingDecisionService
{
    Task<(IEnumerable<FundingDecision>, SearchResult)> GetFundingDecisions(GetFundingDecisionQueryParameters fundingDecisionQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<FundingDecision>, long? searchAfter)> GetFundingDecisionsSearchAfter(GetFundingDecisionQueryParameters fundingDecisionQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
}