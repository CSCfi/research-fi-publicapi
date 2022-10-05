using CSC.PublicApi.Interface.Models;
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
    /// <param name="queryParameters">Julkaisun nimi</param>
    /// <returns></returns>
    [HttpGet(Name = "GetPublication")]
    [Authorize(Policy = ApiPolicies.PublicationSearch)]
    public async Task<IEnumerable<Models.Publication.Publication>> Get([FromQuery] GetPublicationsQueryParameters queryParameters)
    {
        var (publications, searchResult) = await _service.GetPublications(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return publications;
    }
}