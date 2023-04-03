using ResearchFi.Publication;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/publications")]

public class PublicationController : ControllerBase
{
    private readonly ILogger<PublicationController> _logger;
    private IPublicationService _service;

    public PublicationController(
        ILogger<PublicationController> logger,
        IPublicationService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Hae julkaisuja
    /// </summary>
    /// <param name="queryParameters">Julkaisun hakuparametrit</param>
    /// <returns>Taulukko löydettyjä julkaisuja.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Ei autentikoitu.</response>
    /// <response code="403">Ei lupaa suorittaa operaatiota.</response>
    [HttpGet(Name = "SearchPublications")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Publication>> Get([FromQuery] GetPublicationsQueryParameters queryParameters)
    {
        var (publications, searchResult) = await _service.GetPublications(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return publications;
    }

    /// <summary>
    /// Hae julkaisu
    /// </summary>
    /// <param name="publicationId">Julkaisun tunnus</param>
    /// <returns>Löydetty julkaisu.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Ei autentikoitu.</response>
    /// <response code="403">Ei lupaa suorittaa operaatiota.</response>
    /// <response code="404">Julkaisua ei löydy</response>
    [HttpGet("{publicationId}",Name = "GetPublication")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(Publication), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IResult> Get(string publicationId)
    {
        var publication = await _service.GetPublication(publicationId);

        return publication != null ? Results.Ok(publication) : Results.NotFound();
    }
}