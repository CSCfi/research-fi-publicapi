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
    [MapToApiVersion(ApiVersion)]
    public async IAsyncEnumerable<Tilanneraportti> Get([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
         ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext,  queryParameters.PageNumber, queryParameters.PageSize);     
         var tilanneraporttis =  _virtaJtpDbContext.Tilanneraporttis
         .OrderBy(a => a.TilanneraporttiId)
         .AsNoTracking()
         .Where(a => a.TilanneraporttiId > (queryParameters.PageNumber - 1)*queryParameters.PageSize)
         //.Where(b => b.OrganisaatioTunnus == virtaQueryParameters.organisaatiotunnus)
         .Take(queryParameters.PageSize)
         .AsAsyncEnumerable();

         if (virtaQueryParameters.organisaatiotunnus != null)
         {
         tilanneraporttis =  _virtaJtpDbContext.Tilanneraporttis
         .OrderBy(b => b.TilanneraporttiId)
         .AsNoTracking()
         .Where(b => b.TilanneraporttiId > (queryParameters.PageNumber - 1)*queryParameters.PageSize)
         .Where(b => b.OrganisaatioTunnus == virtaQueryParameters.organisaatiotunnus)
         .Take(queryParameters.PageSize)
         .AsAsyncEnumerable();
         }
         
        //var tilanneraporttis = query;

            await foreach (var tilanneraportti in tilanneraporttis)
            {
                yield return tilanneraportti;
            }
       
    }
}
