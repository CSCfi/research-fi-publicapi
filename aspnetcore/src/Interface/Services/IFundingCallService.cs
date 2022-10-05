using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.FundingCall;

namespace CSC.PublicApi.Interface.Services;

public interface IFundingCallService
{
    Task<(IEnumerable<FundingCall>, SearchResult)> GetFundingCalls(GetFundingCallQueryParameters queryParameters);
    Task PostFundCall(FundingCall fundingCall);
}