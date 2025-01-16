using CSC.PublicApi.Interface;
using Microsoft.AspNetCore.Mvc;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using ResearchFi.Query;
//using Organization = ResearchFi.Organization.Organization;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[Route("[controller]")]
public class YhteisjulkaisutRistiriitaisetController : ControllerBase
{
    private const string ApiVersion = "1.0";
    private readonly VirtaJtpDbContext _virtaJtpDbContext;
    private readonly ILogger<YhteisjulkaisutRistiriitaisetController> _logger;

    public YhteisjulkaisutRistiriitaisetController(ILogger<YhteisjulkaisutRistiriitaisetController> logger,VirtaJtpDbContext virtaJtpDbContext)
    {
        _logger = logger;
        _virtaJtpDbContext = virtaJtpDbContext;
    }

    [HttpGet(Name = "GetYhteisjulkaisutRistiriitaiset")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Duplikaatit>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]

    public async IAsyncEnumerable<YhteisjulkaisutRistiriitaiset> Get([FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
         ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);            
         var YhteisjulkaisutRistiriitaisets =  _virtaJtpDbContext.YhteisjulkaisutRistiriitaisets
         .OrderBy(b => b.RrId)
         .AsNoTracking();
 
         YhteisjulkaisutRistiriitaisets = YhteisjulkaisutRistiriitaisets.Skip((queryParameters.PageNumber - 1)*queryParameters.PageSize).Take(queryParameters.PageSize);

            await foreach (var YhteisjulkaisutRistiriitaiset in YhteisjulkaisutRistiriitaisets.AsAsyncEnumerable())
            {
                yield return YhteisjulkaisutRistiriitaiset;
            }
    }
}
