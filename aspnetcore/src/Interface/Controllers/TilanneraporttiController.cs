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
public class TilanneraporttiController : ControllerBase
{    private const string ApiVersion = "1.0";
    private readonly VirtaJtpDbContext _virtaJtpDbContext;
    private readonly ILogger<TilanneraporttiController> _logger;

  //  private string organisaatiotunnus = "10089";

    public TilanneraporttiController(ILogger<TilanneraporttiController> logger,VirtaJtpDbContext virtaJtpDbContext)
    {
        _logger = logger;
        _virtaJtpDbContext = virtaJtpDbContext;
    }

    [HttpGet(Name = "GetTilanneraportti")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Duplikaatit>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async IAsyncEnumerable<Tilanneraportti> Get([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
         ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext,  queryParameters.PageNumber, queryParameters.PageSize);     
         var tilanneraporttis =  _virtaJtpDbContext.Tilanneraporttis
         .OrderBy(a => a.TilanneraporttiId)
         .AsNoTracking();

        if (virtaQueryParameters.organisaatiotunnus != null)
         {
            tilanneraporttis = tilanneraporttis.Where(b =>
            // b.DuplikaattiId > (queryParameters.PageNumber - 1)*queryParameters.PageSize && 
             b.OrganisaatioTunnus == virtaQueryParameters.organisaatiotunnus);
         }
         tilanneraporttis = tilanneraporttis.Skip((queryParameters.PageNumber - 1)*queryParameters.PageSize).Take(queryParameters.PageSize);

            await foreach (var tilanneraportti in tilanneraporttis.AsAsyncEnumerable())
            {
                yield return tilanneraportti;
            }
       
    }
}
