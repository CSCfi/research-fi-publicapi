/*
 * API version 1.0
 */
using CSC.PublicApi.ElasticService;
using ResearchFi.FundingCall;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services.V1;

public interface IFundingCallService
{
    Task<(IEnumerable<FundingCall>, SearchResult)> GetFundingCalls(GetFundingCallQueryParameters fundingCallQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<FundingCall>, SearchAfterResult)> GetFundingCallsSearchAfter(GetFundingCallQueryParameters fundingCallQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
    Task PostFundCall(FundingCall fundingCall);
}