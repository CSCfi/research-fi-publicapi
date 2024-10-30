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
    /// Endpoint for bypassing the limit of 10000 records for publications.
    /// </summary>
    /// <param name="publicationsQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="Publication"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetPublicationsExport")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Publication>> Get([FromQuery] GetPublicationsQueryParameters publicationsQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (publications, searchAfterResult) = await _service.GetPublicationsSearchAfter(publicationsQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfterResult);

        return publications;
    }
}