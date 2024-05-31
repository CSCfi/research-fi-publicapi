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
    public async Task<List<YhteisjulkaisutRistiriitaiset>> Get()
    {

        List <YhteisjulkaisutRistiriitaiset> yhteisjulkaisutRistiriitaiset = await _virtaJtpDbContext.YhteisjulkaisutRistiriitaisets.ToListAsync();
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


        return yhteisjulkaisutRistiriitaiset;
      /*  return Enumerable.Range(1, 5).Select(index => new Virheraportti
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = 21, //Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })  
        .ToArray(); */
    }
}
