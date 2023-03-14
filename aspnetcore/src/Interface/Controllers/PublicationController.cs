using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Publication;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/publications")]

public class PublicationController : ControllerBase
{
    private const string ApiVersion = "1.0";

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
    /// <returns></returns>
    [HttpGet(Name = "SearchPublications")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
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
    /// <returns></returns>
    [HttpGet("{publicationId}",Name = "GetPublication")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [ProducesResponseType(typeof(Publication), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> Get(string publicationId)
    {
        var publication = await _service.GetPublication(publicationId);

        return publication != null ? Results.Ok(publication) : Results.NotFound();
    }
}