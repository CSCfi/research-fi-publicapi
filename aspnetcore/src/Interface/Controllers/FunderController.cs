using CSC.PublicApi.Interface.Models.FundersAPI.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpPublicApiFundersGrantedFunding = CSC.PublicApi.Interface.Models.FundersAPI.ImportDb.Entities.ImpPublicApiFundersGrantedFunding;
using ImpPublicApiFundersGrantedFundingPublication = CSC.PublicApi.Interface.Models.FundersAPI.ImportDb.Entities.ImpPublicApiFundersGrantedFundingPublication;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/funders")]
public class FunderController : ControllerBase
{
    private readonly ImportDbContext _importDbContext;
    private readonly ILogger<FunderController> _logger;

    public FunderController(ImportDbContext importDbContext, ILogger<FunderController> logger)
    {
        _importDbContext = importDbContext;
        _logger = logger;
    }

    /// <summary>
    /// List granted fundings.
    /// </summary>
    /// <returns>Collection of <see cref="GrantedFunding"/></returns>
    [HttpGet(Name = "GetGrantedFundings")]
    [ProducesResponseType(typeof(IEnumerable<GrantedFunding>), StatusCodes.Status200OK)]
    public async IAsyncEnumerable<GrantedFunding> GetGrantedFundings()
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var granteFundingEntities = _importDbContext.ImpPublicApiFundersGrantedFundings.Where(e => e.ClientId == clientId).AsNoTracking();
        await foreach (var granteFundingEntity in granteFundingEntities.AsAsyncEnumerable())
        {
            yield return new GrantedFunding
            {
                GrantedFundingId = granteFundingEntity.FunderProjectNumber,
                OrganizationId = granteFundingEntity.OrganizationId
            };
        }
    }

    /// <summary>
    /// Add granted funding.
    /// </summary>
    /// <param name="grantedFundingModel">Granted funding details provided in the request body.</param>
    /// <returns><see cref="GrantedFunding"/></returns>
    [HttpPost(Name = "PostGrantedFunding")]
    [ProducesResponseType(typeof(GrantedFunding), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PostGrantedFunding([FromBody] GrantedFunding grantedFundingModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        if (await _importDbContext.ImpPublicApiFundersGrantedFundings.AnyAsync(grantedFunding =>
            grantedFunding.ClientId == clientId &&
            grantedFunding.FunderProjectNumber == grantedFundingModel.GrantedFundingId))
        {
            return Conflict("Already exists");
        }
        ImpPublicApiFundersGrantedFunding grantedFunding = new ImpPublicApiFundersGrantedFunding
        {
            ClientId = clientId,
            FunderProjectNumber = grantedFundingModel.GrantedFundingId,
            OrganizationId = grantedFundingModel.OrganizationId,
            Created = DateTime.UtcNow
        };
        _importDbContext.ImpPublicApiFundersGrantedFundings.Add(grantedFunding);
        await _importDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGrantedFunding), new { grantedFundingId = grantedFunding.FunderProjectNumber }, grantedFundingModel);
    }

    /// <summary>
    /// Get granted funding.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <returns><see cref="GrantedFunding"/></returns>
    [HttpGet("{grantedFundingId}", Name = "GetGrantedFunding")]
    [ProducesResponseType(typeof(GrantedFunding), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrantedFunding(string grantedFundingId)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var granteFundingEntity = await _importDbContext.ImpPublicApiFundersGrantedFundings
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.ClientId == clientId && f.FunderProjectNumber == grantedFundingId);
        if (granteFundingEntity == null)
        {
            return NotFound();
        }
        var grantedFunding = new GrantedFunding
        {
            GrantedFundingId = granteFundingEntity.FunderProjectNumber,
            OrganizationId = granteFundingEntity.OrganizationId
        };
        return Ok(grantedFunding);
    }

    /// <summary>
    /// Modify granted funding.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="grantedFundingModel">Granted funding details provided in the request body.</param>
    /// <returns><see cref="GrantedFunding"/></returns>
    [HttpPut("{grantedFundingId}", Name = "PutGrantedFunding")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutGrantedFunding(string grantedFundingId, [FromBody] GrantedFunding grantedFundingModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var granteFundingEntity = await _importDbContext.ImpPublicApiFundersGrantedFundings.FirstOrDefaultAsync(f => f.ClientId == clientId && f.FunderProjectNumber == grantedFundingId);
        if (granteFundingEntity == null)
        {
            return NotFound();
        }
        granteFundingEntity.FunderProjectNumber = grantedFundingModel.GrantedFundingId;
        granteFundingEntity.OrganizationId = grantedFundingModel.OrganizationId;
        granteFundingEntity.Modified = DateTime.UtcNow;
        await _importDbContext.SaveChangesAsync();
        return Ok(grantedFundingModel);
    }

    /// <summary>
    /// Delete granted funding. Deletes all related resources.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    [HttpDelete("{grantedFundingId}", Name = "DeleteGrantedFunding")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGrantedFunding(string grantedFundingId)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var granteFundingEntity = await _importDbContext.ImpPublicApiFundersGrantedFundings
            .Include(g => g.ImpPublicApiFundersGrantedFundingPublications)
            .FirstOrDefaultAsync(f => f.ClientId == clientId && f.FunderProjectNumber == grantedFundingId);
        if (granteFundingEntity == null)
        {
            return NotFound();
        }
        _importDbContext.ImpPublicApiFundersGrantedFundingPublications.RemoveRange(granteFundingEntity.ImpPublicApiFundersGrantedFundingPublications);
        _importDbContext.ImpPublicApiFundersGrantedFundings.Remove(granteFundingEntity);
        await _importDbContext.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// List granted funding related publications.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <returns>Collection of <see cref="GrantedFundingPublication"/></returns>
    [HttpGet("{grantedFundingId}/publications", Name = "GetGrantedFundingPublications")]
    [ProducesResponseType(typeof(IEnumerable<GrantedFundingPublication>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IEnumerable<GrantedFundingPublication>> GetGrantedFundingPublications(string grantedFundingId)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var granteFundingEntity = await _importDbContext.ImpPublicApiFundersGrantedFundings
            .Include(g => g.ImpPublicApiFundersGrantedFundingPublications)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.ClientId == clientId && g.FunderProjectNumber == grantedFundingId);
        if (granteFundingEntity == null)
        {
            return Enumerable.Empty<GrantedFundingPublication>();
        }
        return granteFundingEntity.ImpPublicApiFundersGrantedFundingPublications.Select(publication => new GrantedFundingPublication
        {
            PublicationId = publication.PublicationId,
            Doi = publication.Doi
        });
    }

    /// <summary>
    /// Add granted funding related publication.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="grantedFundingPublicationModel">Publication details provided in the request body.</param>
    /// <returns><see cref="GrantedFundingPublication"/></returns>
    [HttpPost("{grantedFundingId}/publications", Name = "PostGrantedFundingPublication")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PostGrantedFundingPublication(string grantedFundingId, [FromBody] GrantedFundingPublication grantedFundingPublicationModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var grantedFundingEntity = await _importDbContext.ImpPublicApiFundersGrantedFundings
            .FirstOrDefaultAsync(g => g.ClientId == clientId && g.FunderProjectNumber == grantedFundingId);
        if (
            await _importDbContext.ImpPublicApiFundersGrantedFundingPublications
                .AnyAsync(publication =>
                    publication.FkGrantedFundingNavigation == grantedFundingEntity &&
                    publication.ClientId == clientId &&
                    publication.PublicationId == grantedFundingPublicationModel.PublicationId &&
                    publication.Doi == grantedFundingPublicationModel.Doi)
        )
        {
            return Conflict("Already exists");
        }
        var publicationEntity = new ImpPublicApiFundersGrantedFundingPublication
        {
            FkGrantedFundingNavigation = grantedFundingEntity,
            ClientId = clientId,
            Created = DateTime.UtcNow
        };
        if (!string.IsNullOrEmpty(grantedFundingPublicationModel.PublicationId))
        {
            publicationEntity.PublicationId = grantedFundingPublicationModel.PublicationId;
        }
        if (!string.IsNullOrEmpty(grantedFundingPublicationModel.Doi))
        {
            publicationEntity.Doi = grantedFundingPublicationModel.Doi;
        }
        _importDbContext.ImpPublicApiFundersGrantedFundingPublications.Add(publicationEntity);
        await _importDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGrantedFundingPublication), new { grantedFundingId = grantedFundingEntity.FunderProjectNumber, publicationIdentifier = grantedFundingPublicationModel.GetPublicationIdentifier() }, grantedFundingPublicationModel);
    }

    /// <summary>
    /// Get granted funding related publication.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="publicationIdentifier">Publication Identifier.</param>
    /// <returns><see cref="GrantedFundingPublication"/></returns>
    [HttpGet("{grantedFundingId}/publications/{publicationIdentifier}", Name = "GetGrantedFundingPublication")]
    [ProducesResponseType(typeof(GrantedFundingPublication), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrantedFundingPublication(string grantedFundingId, string publicationIdentifier)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var publicationEntity = await _importDbContext.ImpPublicApiFundersGrantedFundingPublications
            .Include(p => p.FkGrantedFundingNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.ClientId == clientId &&
                p.FkGrantedFundingNavigation.FunderProjectNumber == grantedFundingId &&
                p.PublicationId == publicationIdentifier || p.Doi == publicationIdentifier);
        if (publicationEntity == null)
        {
            return NotFound();
        }
        var publication = new GrantedFundingPublication
        {
            PublicationId = publicationEntity.PublicationId,
            Doi = publicationEntity.Doi
        };
        return Ok(publication);
    }

    /// <summary>
    /// Modify granted funding related publication.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="publicationIdentifier">Publication identifier.</param>
    /// <param name="grantedFundingPublicationModel">Publication details provided in the request body.</param>
    /// <returns><see cref="GrantedFunding"/></returns>
    [HttpPut("{grantedFundingId}/publications/{publicationIdentifier}", Name = "PutGrantedFundingPublication")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutGrantedFundingPublication(string grantedFundingId, string publicationIdentifier, [FromBody] GrantedFundingPublication grantedFundingPublicationModel)
    {
        if (grantedFundingPublicationModel == null || string.IsNullOrEmpty(grantedFundingPublicationModel.PublicationId) && string.IsNullOrEmpty(grantedFundingPublicationModel.Doi))
        {
            return BadRequest("Either PublicationId or Doi is required");
        }
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var publicationEntity = await _importDbContext.ImpPublicApiFundersGrantedFundingPublications
            .Include(p => p.FkGrantedFundingNavigation)
            .FirstOrDefaultAsync(p =>
                p.ClientId == clientId &&
                p.FkGrantedFundingNavigation.FunderProjectNumber == grantedFundingId &&
                p.PublicationId == publicationIdentifier || p.Doi == publicationIdentifier);
        if (publicationEntity == null)
        {
            return NotFound();
        }
        publicationEntity.PublicationId = grantedFundingPublicationModel.PublicationId;
        publicationEntity.Doi = grantedFundingPublicationModel.Doi;
        publicationEntity.Modified = DateTime.UtcNow;
        await _importDbContext.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Delete granted funding related publication.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="publicationIdentifier">Publication identifier.</param>
    [HttpDelete("{grantedFundingId}/publications/{publicationIdentifier}", Name = "DeleteGrantedFundingPublication")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGrantedFundingPublication(string grantedFundingId, string publicationIdentifier)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var publicationEntity = await _importDbContext.ImpPublicApiFundersGrantedFundingPublications
            .Include(p => p.FkGrantedFundingNavigation)
            .FirstOrDefaultAsync(p =>
                p.ClientId == clientId &&
                p.FkGrantedFundingNavigation.FunderProjectNumber == grantedFundingId &&
                p.PublicationId == publicationIdentifier || p.Doi == publicationIdentifier);
        if (publicationEntity == null)
        {
            return NotFound();
        }
        _importDbContext.ImpPublicApiFundersGrantedFundingPublications.Remove(publicationEntity);
        await _importDbContext.SaveChangesAsync();
        return NoContent();
    }
}