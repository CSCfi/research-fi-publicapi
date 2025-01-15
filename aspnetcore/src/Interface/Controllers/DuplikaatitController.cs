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
public class DuplikaatitController : ControllerBase
{    private const string ApiVersion = "1.0";
    private readonly VirtaJtpDbContext _virtaJtpDbContext;
    private readonly ILogger<DuplikaatitController> _logger;

    public DuplikaatitController(ILogger<DuplikaatitController> logger,VirtaJtpDbContext virtaJtpDbContext)
    {
        _logger = logger;
        _virtaJtpDbContext = virtaJtpDbContext;
    }

    [HttpGet(Name = "GetDuplikaatit")]
    [MapToApiVersion(ApiVersion)]
    public async IAsyncEnumerable<Duplikaatit> Get([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
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
}
