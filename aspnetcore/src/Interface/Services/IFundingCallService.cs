using CSC.PublicApi.ElasticService;
using ResearchFi.FundingCall;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public interface IFundingCallService
{
    Task<(IEnumerable<FundingCall>, SearchResult)> GetFundingCalls(GetFundingCallQueryParameters queryParameters);
    Task PostFundCall(FundingCall fundingCall);
}