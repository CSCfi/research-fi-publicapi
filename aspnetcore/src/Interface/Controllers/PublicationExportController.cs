using ResearchFi.Publication;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/publications-export")]

public class PublicationExportController : ControllerBase
{
    private readonly ILogger<PublicationController> _logger;
    private IPublicationService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public PublicationExportController(
        ILogger<PublicationController> logger,
        IPublicationService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
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
    [HttpGet(Name = "SearchPublications2")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Publication>> Get([FromQuery] GetPublicationsQueryParameters2 searchAfterQueryParameters)
    {
        var (publications, searchAfter) = await _service.GetPublicationsSearchAfter(searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return publications;
    }
}