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

public class VirtaJulkaisutietopalveluController : ControllerBase
{    private const string ApiVersion = "1.0";
    private readonly VirtaJtpDbContext _virtaJtpDbContext;
    private readonly ILogger<VirtaJulkaisutietopalveluController> _logger;

    public VirtaJulkaisutietopalveluController(ILogger<VirtaJulkaisutietopalveluController> logger,VirtaJtpDbContext virtaJtpDbContext)
    {
        _logger = logger;
        _virtaJtpDbContext = virtaJtpDbContext;
    }

    [HttpGet("Latausraportit/duplikaatit")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Duplikaatit>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]    
    public async IAsyncEnumerable<Duplikaatit> GetDuplikaatit([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
         ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);     
         var duplikaatits =  _virtaJtpDbContext.Duplikaatits
         .OrderBy(b => b.DuplikaattiId)
         .AsNoTracking();
         //.Where(b => b.DuplikaattiId > (queryParameters.PageNumber - 1)*queryParameters.PageSize)
         
        if (virtaQueryParameters.organisaatiotunnus != null)
         {
            duplikaatits = duplikaatits.Where(b =>
            // b.DuplikaattiId > (queryParameters.PageNumber - 1)*queryParameters.PageSize && 
             b.Organisaatiotunnus == virtaQueryParameters.organisaatiotunnus);
         }
         duplikaatits = duplikaatits.Skip((queryParameters.PageNumber - 1)*queryParameters.PageSize).Take(queryParameters.PageSize);
 

            await foreach (var duplikaatti in duplikaatits.AsAsyncEnumerable())
            {
                yield return duplikaatti;
            }
    }

    [HttpGet("Latausraportit/tilanne")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Tilanneraportti>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]

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

    [HttpGet("Latausraportit/virheet")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Virheraportti>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]

    public async IAsyncEnumerable<Virheraportti> GetVirheet([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
         ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);     
         var virheraporttis =  _virtaJtpDbContext.Virheraporttis
         .OrderBy(b => b.VirheraporttiId)
         .AsNoTracking();

        if (virtaQueryParameters.organisaatiotunnus != null)
         {
            virheraporttis = virheraporttis.Where(b =>
            // b.DuplikaattiId > (queryParameters.PageNumber - 1)*queryParameters.PageSize && 
             b.OrganisaatioTunnus == virtaQueryParameters.organisaatiotunnus);
         }
         virheraporttis = virheraporttis.Skip((queryParameters.PageNumber - 1)*queryParameters.PageSize).Take(queryParameters.PageSize);
 

            await foreach (var virheraportti in virheraporttis.AsAsyncEnumerable())
            {
                yield return virheraportti;
            }   
    }

    [HttpGet("Yhteisjulkaisut/ristiriitaiset")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<YhteisjulkaisutRistiriitaiset>), StatusCodes.Status200OK)]
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
