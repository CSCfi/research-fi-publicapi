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
using CSC.PublicApi.Interface.Models.ImportDb.ApiModels;
using CSC.PublicApi.Interface.Models.ImportDb.Entities;
using System.ComponentModel.DataAnnotations;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[Route("v{version:apiVersion}/links")] 

public class LinkController : ControllerBase
{    
    private const string ApiVersion = "1.0";
    private readonly ImportDbContext _importDbContext;
    private readonly ILogger<LinkController> _logger;

    public LinkController(ILogger<LinkController> logger, ImportDbContext importDbContext)
    {
        _logger = logger;
        _importDbContext = importDbContext;
    }

    [HttpGet("/")]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Duplikaatit>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]    
    public async IAsyncEnumerable<PublicationToGrantedFunding> GetPublicationToGrantedFunding()
    { 
        var items =  _importDbContext.ImpLinkPublicationToGrantedFundings.AsNoTracking(); 
        await foreach (var item in items.AsAsyncEnumerable())
        {
            yield return new PublicationToGrantedFunding
            {
                PublicationId = item.PublicationId,
                Doi = item.Doi,
                OrganizationId = item.OrganizationId,
                FunderProjectNumber = item.FunderProjectNumber
            };
        }
    }


    [HttpPost]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddPublicationToGrantedFunding([FromBody] PublicationToGrantedFunding publicationToGrantedFunding)
    {
        try
        {
            var entity = new ImpLinkPublicationToGrantedFunding
            {
                PublicationId = publicationToGrantedFunding.PublicationId,
                Doi = publicationToGrantedFunding.Doi,
                OrganizationId = publicationToGrantedFunding.OrganizationId,
                FunderProjectNumber = publicationToGrantedFunding.FunderProjectNumber,
                ClientId = HttpContext.Items["ClientId"]?.ToString(),
                Created = DateTime.Now
            };

            _importDbContext.ImpLinkPublicationToGrantedFundings.Add(entity);
            await _importDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPublicationToGrantedFunding), new { id = entity.PublicationId }, publicationToGrantedFunding);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding PublicationToGrantedFunding.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}
