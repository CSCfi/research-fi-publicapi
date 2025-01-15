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
    [MapToApiVersion(ApiVersion)]
    public async IAsyncEnumerable<YhteisjulkaisutRistiriitaiset> Get([FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
         ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);            
         var YhteisjulkaisutRistiriitaisets =  _virtaJtpDbContext.YhteisjulkaisutRistiriitaisets
         .OrderBy(b => b.RrId)
         .AsNoTracking()
         .Where(b => b.RrId > (queryParameters.PageNumber - 1)*queryParameters.PageSize)
         .Take(queryParameters.PageSize)
         .AsAsyncEnumerable();

            await foreach (var YhteisjulkaisutRistiriitaiset in YhteisjulkaisutRistiriitaisets)
            {
                yield return YhteisjulkaisutRistiriitaiset;
            }
        
    }
}
