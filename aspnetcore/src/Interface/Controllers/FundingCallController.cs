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
    private readonly IDiagnosticContext _diagnosticContext;

    public FundingCallController(
        ILogger<FundingCallController> logger,
        IFundingCallService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_FundingCall);
    }

    /// <summary>
    /// Endpoint for filtering funding calls using the specified query parameters.
    /// </summary>
    /// <param name="fundingCallQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="FundingCall"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetFundingCall")]
    [MapToApiVersion(ApiConstants.ApiVersion1)]
    [Authorize(Policy = ApiPolicies.FundingCall.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingCall>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<FundingCall>> Get([FromQuery] GetFundingCallQueryParameters fundingCallQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (fundingCalls, searchResult) = await _service.GetFundingCalls(fundingCallQueryParameters, paginationQueryParameters);

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