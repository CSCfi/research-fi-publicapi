using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.FundingCall;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/funding-calls")]
public class FundingCallController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ILogger<FundingCallController> _logger;
    private readonly IFundingCallService _service;

    public FundingCallController(
        ILogger<FundingCallController> logger,
        IFundingCallService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Hae rahoitushakuja
    /// </summary>
    /// <param name="queryParameters">Query parameters for filtering the results.</param>
    /// <returns>Filtered and paginated results as an enumerated list of <see cref="FundingCall"/>.</returns>
    [HttpGet(Name = "GetFundingCall")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.FundingCall.Read)]
    public async Task<IEnumerable<FundingCall>> Get([FromQuery] GetFundingCallQueryParameters queryParameters)
    {
        var (fundingCalls, searchResult) = await _service.GetFundingCalls(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return fundingCalls;
    }

    /// <summary>
    /// Lisää uusi rahoitushaku
    /// </summary>
    /// <param name="fundingCall"></param>
    /// <returns></returns>
    [HttpPost(Name = "PostFundingCall")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.FundingCall.Write)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task Post(FundingCall fundingCall)
    {
        await _service.PostFundCall(fundingCall);
    }
}