using CSC.PublicApi.ElasticService;
using ResearchFi.FundingCall;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IFundingCallService
{
    Task<(IEnumerable<FundingCall>, SearchResult)> GetFundingCalls(GetFundingCallQueryParameters fundingCallQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<FundingCall>, long? searchAfter)> GetFundingCallsSearchAfter(GetFundingCallQueryParameters fundingCallQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
    Task PostFundCall(FundingCall fundingCall);
}