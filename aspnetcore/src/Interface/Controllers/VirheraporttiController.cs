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
public class VirheraporttiController : ControllerBase
{    private const string ApiVersion = "1.0";
    private readonly VirtaJtpDbContext _virtaJtpDbContext;
    private readonly ILogger<VirheraporttiController> _logger;

    public VirheraporttiController(ILogger<VirheraporttiController> logger,VirtaJtpDbContext virtaJtpDbContext)
    {
        _logger = logger;
        _virtaJtpDbContext = virtaJtpDbContext;
    }

    [HttpGet(Name = "GetVirheraportti")]
    [MapToApiVersion(ApiVersion)]
    public async IAsyncEnumerable<Virheraportti> Get([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
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
}
