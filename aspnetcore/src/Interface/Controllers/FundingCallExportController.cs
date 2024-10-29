using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.FundingCall;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/funding-calls-export")]
public class FundingCallExportController : ControllerBase
{
    private readonly ILogger<FundingCallExportController> _logger;
    private readonly IFundingCallService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public FundingCallExportController(
        ILogger<FundingCallExportController> logger,
        IFundingCallService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_FundingCall);
    }

    /// <summary>
    /// Endpoint for bypassing the limit of 10000 records for funding calls.
    /// </summary>
    /// <param name="queryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="FundingCall"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetFundingCallExport")]
    [MapToApiVersion(ApiConstants.ApiVersion1)]
    [Authorize(Policy = ApiPolicies.FundingCall.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingCall>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<FundingCall>> Get([FromQuery] GetFundingCallQueryParameters fundingCallQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (fundingCalls, searchAfter) = await _service.GetFundingCallsSearchAfter(fundingCallQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return fundingCalls;
    }
}