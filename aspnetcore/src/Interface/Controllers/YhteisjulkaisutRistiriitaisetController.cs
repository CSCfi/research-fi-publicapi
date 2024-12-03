using CSC.PublicApi.Interface;
using Microsoft.AspNetCore.Mvc;
using CSC.PublicApi.Interface.Models;

using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using ResearchFi.Query;
//using Organization = ResearchFi.Organization.Organization;
using Serilog;
using Microsoft.EntityFrameworkCore;


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
   // [Route("/[controller]/etunimet/{etunimi}")]
    //public async Task<List<YhteisjulkaisutRistiriitaiset>> Get()
    public async IAsyncEnumerable<YhteisjulkaisutRistiriitaiset> Get([FromQuery] VirtaPaginationQueryParameters queryParameters)
    
    {
        var YhteisjulkaisutRistiriitaisets =  _virtaJtpDbContext.YhteisjulkaisutRistiriitaisets
        //List <YhteisjulkaisutRistiriitaiset> yhteisjulkaisutRistiriitaiset = await _virtaJtpDbContext.YhteisjulkaisutRistiriitaisets.
         .OrderBy(b => b.RrId)
         .AsNoTracking()
         .Where(b => b.RrId > (queryParameters.PageNumber - 1)*queryParameters.PageSize)
         .Take(queryParameters.PageSize)
         .AsAsyncEnumerable();

            await foreach (var YhteisjulkaisutRistiriitaiset in YhteisjulkaisutRistiriitaisets)
            {
                yield return YhteisjulkaisutRistiriitaiset;
            }
        ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);
             
        //ToListAsync();
       /* List <DisplayTest> displayTests = new(); 
        foreach(TableTest t in tableTests)
        {
            displayTests.Add(
                new DisplayTest(){
                    Etunimi = t.Etunimi, 
                    Sukunimi = t.Sukunimi
                }
            );
        }*/
       // return yhteisjulkaisutRistiriitaiset;
    }
}
