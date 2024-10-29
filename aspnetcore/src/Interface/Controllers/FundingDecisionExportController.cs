using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.FundingDecision;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/funding-decisions-export")]

public class FundingDecisionExportController : ControllerBase
{
    private const string ApiVersion = "1.0";
    private readonly ILogger<FundingDecisionExportController> _logger;
    private readonly IFundingDecisionService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public FundingDecisionExportController(
        ILogger<FundingDecisionExportController> logger,
        IFundingDecisionService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_FundingDecision);
    }

    /// <summary>
    /// Endpoint for bypassing the limit of 10000 records for funding decisions.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="FundingDecision"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetFundingDecisionExport")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.FundingDecision.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingDecision>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<FundingDecision>> Get([FromQuery] GetFundingDecisionQueryParameters fundingDecisionQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (fundingDecisions, searchAfter) = await _service.GetFundingDecisionsSearchAfter(fundingDecisionQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return fundingDecisions;
    }
}