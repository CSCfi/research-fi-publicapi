using ResearchFi.Publication;
using ResearchFi.FundingDecision;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/export")]

public class ExportController : ControllerBase
{
    private readonly ILogger<PublicationController> _logger;
    private readonly IPublicationService _publicationService;
    private readonly IFundingDecisionService _fundingDecisionService;
    private readonly IDiagnosticContext _diagnosticContext;

    public ExportController(
        ILogger<PublicationController> logger,
        IPublicationService publicationService,
        IFundingDecisionService fundingDecisionService,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _publicationService = publicationService;
        _fundingDecisionService =fundingDecisionService;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_Publication);
    }

    /// <summary>
    /// Experimental endpoint for getting more than 10000 publications.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="Publication"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("publications")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Publication>> Get([FromQuery] GetPublicationsQueryParameters publicationsQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (publications, searchAfter) = await _publicationService.GetPublicationsSearchAfter(publicationsQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return publications;
    }


    /// <summary>
    /// Experimental endpoint for getting more than 10000 funding decisions.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="FundingDecision"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("funding-decisions")]
    [Authorize(Policy = ApiPolicies.FundingDecision.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingDecision>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<FundingDecision>> Get([FromQuery] GetFundingDecisionQueryParameters fundingDecisionQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (fundingDecisions, searchResult) = await _fundingDecisionService.GetFundingDecisions(fundingDecisionQueryParameters, paginationQueryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return fundingDecisions;
    }
}