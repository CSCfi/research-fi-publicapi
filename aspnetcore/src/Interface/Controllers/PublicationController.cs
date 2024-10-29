using ResearchFi.Publication;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/publications")]

public class PublicationController : ControllerBase
{
    private readonly ILogger<PublicationController> _logger;
    private IPublicationService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public PublicationController(
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
    /// Endpoint for filtering publications using the specified query parameters.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="Publication"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetPublications")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Publication>> Get([FromQuery] GetPublicationsQueryParameters publicationsQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (publications, searchResult) = await _service.GetPublications(publicationsQueryParameters, paginationQueryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return publications;
    }

    /// <summary>
    /// Endpoint for getting a single publication based on the ID.
    /// </summary>
    /// <param name="publicationId">ID of the publication.</param>
    /// <returns>An instance of <see cref="Publication"/>.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="404">Not found.</response>
    [HttpGet("{publicationId}", Name = "GetPublication")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(Publication), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IResult> Get(string publicationId)
    {
        var publication = await _service.GetPublication(publicationId);

        return publication != null ? Results.Ok(publication) : Results.NotFound();
    }
}