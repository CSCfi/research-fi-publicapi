using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.FundingCall;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/funding-calls")]
public class FundingCallController : ControllerBase
{
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
    /// Metodi rahoitushakujen suodattamiseen määritellyillä hakuehdoilla.
    /// </summary>
    /// <param name="queryParameters">Hakuehdot, joiden perusteella tuloksia suodatetaan.</param>
    /// <returns>Sivutettu hakutulos joka koostuu kokoelmasta <see cref="FundingCall"/> objekteja.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Ei autentikoitu.</response>
    /// <response code="403">Ei lupaa suorittaa operaatiota.</response>
    [HttpGet(Name = "GetFundingCall")]
    [MapToApiVersion(ApiConstants.ApiVersion1)]
    [Authorize(Policy = ApiPolicies.FundingCall.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingCall>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
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
    [MapToApiVersion(ApiConstants.ApiVersion1)]
    [Authorize(Policy = ApiPolicies.FundingCall.Write)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task Post(FundingCall fundingCall)
    {
        await _service.PostFundCall(fundingCall);
    }
}