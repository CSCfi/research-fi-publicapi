using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/funding-decisions")]

public class FundingDecisionController : ControllerBase
{
    private const string ApiVersion = "1.0";
    private readonly ILogger<FundingDecisionController> _logger;
    private readonly IFundingDecisionService _service;

    public FundingDecisionController(
        ILogger<FundingDecisionController> logger,
        IFundingDecisionService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Hae rahoituspäätöksiä
    /// </summary>
    /// <param name="queryParameters">Query parameters for filtering the results.</param>
    /// <returns></returns>
    [HttpGet(Name = "GetFundingDecision")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.FundingDecision.Read)]
    public async Task<IEnumerable<Models.FundingDecision.FundingDecision>> Get([FromQuery] GetFundingDecisionQueryParameters queryParameters)
    {
        var (fundingDecisions, searchResult) = await _service.GetFundingDecisions(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return fundingDecisions;
    }
}