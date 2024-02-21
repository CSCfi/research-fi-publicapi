using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.FundingDecision;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/funding-decisions")]

public class FundingDecisionController : ControllerBase
{
    private const string ApiVersion = "1.0";
    private readonly ILogger<FundingDecisionController> _logger;
    private readonly IFundingDecisionService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public FundingDecisionController(
        ILogger<FundingDecisionController> logger,
        IFundingDecisionService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_FundingDecision);
    }

    /// <summary>
    /// Endpoint for filtering funding decisions using the specified query parameters.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="FundingDecision"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetFundingDecision")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.FundingDecision.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingDecision>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<FundingDecision>> Get([FromQuery] GetFundingDecisionQueryParameters queryParameters)
    {
        var (fundingDecisions, searchResult) = await _service.GetFundingDecisions(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return fundingDecisions;
    }
}