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
   // [Route("/[controller]/etunimet/{etunimi}")]
    public async IAsyncEnumerable<Virheraportti> Get([FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
        

/*        List <Virheraportti> virheraportti = await _virtaJtpDbContext.Virheraporttis.OrderBy(b => b.VirheraporttiId)
            .AsNoTracking()
            .Where(b => b.VirheraporttiId > (queryParameters.PageNumber - 1)*queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .AsAsyncEnumerable();
*/
            var virheraporttis =  _virtaJtpDbContext.Virheraporttis
            .AsAsyncEnumerable();

            await foreach (var virheraportti in virheraporttis)
            {
                yield return virheraportti;
            }
        //ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);
     

        //return virheraportti;
    }
}
