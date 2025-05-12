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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GrantedFunding>), StatusCodes.Status200OK)]
    public async IAsyncEnumerable<GrantedFunding> GetGrantedFundings()
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entities = _importDbContext.ImpPublicApiFundersGrantedFundings.Where(e => e.ClientId == clientId).AsNoTracking();

        await foreach (var entity in entities.AsAsyncEnumerable())
        {
            yield return new GrantedFunding
            {
                GrantedFundingId = entity.FunderProjectNumber
            };
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PostGrantedFunding([FromBody] GrantedFunding grantedFundingFromApi)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;

        if (await _importDbContext.ImpPublicApiFundersGrantedFundings.AnyAsync(grantedFunding => grantedFunding.ClientId == clientId && grantedFunding.FunderProjectNumber == grantedFundingFromApi.GrantedFundingId))
        {
            return Conflict("Already exists: " + grantedFundingFromApi.GrantedFundingId);
        }

        ImpPublicApiFundersGrantedFunding grantedFunding = new ImpPublicApiFundersGrantedFunding
        {
            ClientId = clientId,
            FunderProjectNumber = grantedFundingFromApi.GrantedFundingId,
            OrganizationId = "foo",
            Created = DateTime.UtcNow
        };

        _importDbContext.ImpPublicApiFundersGrantedFundings.Add(grantedFunding);
        await _importDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGrantedFunding), new { grantedFundingId = grantedFunding.FunderProjectNumber }, grantedFundingFromApi);
    }

    [HttpGet("{grantedFundingId}")]
    [ProducesResponseType(typeof(GrantedFunding), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrantedFunding(string grantedFundingId)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpPublicApiFundersGrantedFundings
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.ClientId == clientId && f.FunderProjectNumber == grantedFundingId);

        if (entity == null)
        {
            return NotFound();
        }

        var grantedFunding = new GrantedFunding
        {
            GrantedFundingId = entity.FunderProjectNumber
        };

        return Ok(grantedFunding);
    }

    [HttpPut("{grantedFundingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutGrantedFunding(string grantedFundingId, [FromBody] GrantedFunding grantedFundingFromApi)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpPublicApiFundersGrantedFundings.FirstOrDefaultAsync(f => f.ClientId == clientId && f.FunderProjectNumber == grantedFundingId);
        if (entity == null)
        {
            return NotFound();
        }

        entity.FunderProjectNumber = grantedFundingFromApi.GrantedFundingId;
        entity.Modified = DateTime.UtcNow;

        await _importDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{grantedFundingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGrantedFunding(string grantedFundingId)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpPublicApiFundersGrantedFundings.FirstOrDefaultAsync(f => f.ClientId == clientId && f.FunderProjectNumber == grantedFundingId);
        if (entity == null)
        {
            return NotFound();
        }

        _importDbContext.ImpPublicApiFundersGrantedFundings.Remove(entity);
        await _importDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{grantedFundingId}/publications")]
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

    [HttpPost("{grantedFundingId}/publications")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostGetGrantedFundingPublication(string grantedFundingId, [FromBody] GrantedFundingPublication grantedFundingPublicationModel)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var grantedFundingEntity = await _importDbContext.ImpPublicApiFundersGrantedFundings
            .FirstOrDefaultAsync(g => g.ClientId == clientId && g.FunderProjectNumber == grantedFundingId);

        var publicationEntity = new ImpPublicApiFundersGrantedFundingPublication
        {
            FkGrantedFundingNavigation = grantedFundingEntity,    
            ClientId = clientId,
            PublicationId = grantedFundingPublicationModel.PublicationId,
            Doi = grantedFundingPublicationModel.Doi,
            Created = DateTime.UtcNow
        };

        _importDbContext.ImpPublicApiFundersGrantedFundingPublications.Add(publicationEntity);
        await _importDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGrantedFundingPublication), new { grantedFundingId = grantedFundingEntity.FunderProjectNumber, publicationIdentifier = grantedFundingPublicationModel.GetPublicationIdentifier() }, grantedFundingPublicationModel);
    }

    [HttpGet("{grantedFundingId}/publications/{publicationIdentifier}")]
    [ProducesResponseType(typeof(GrantedFundingPublication), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrantedFundingPublication(string grantedFundingId, string publicationIdentifier)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpPublicApiFundersGrantedFundingPublications
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ClientId == clientId && p.PublicationId == publicationIdentifier);

        if (entity == null)
        {
            return NotFound();
        }

        var publication = new GrantedFundingPublication
        {
            PublicationId = entity.PublicationId,
            Doi = entity.Doi
        };

        return Ok(publication);
    }

    [HttpPut("{grantedFundingId}/publications/{publicationIdentifier}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutGrantedFundingPublication(string grantedFundingId, string publicationIdentifier, [FromBody] GrantedFundingPublication model)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpPublicApiFundersGrantedFundingPublications.FindAsync(publicationIdentifier);

        if (entity == null)
        {
            return NotFound();
        }

        entity.PublicationId = model.PublicationId;
        entity.Doi = model.Doi;
        entity.Modified = DateTime.UtcNow;

        await _importDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{grantedFundingId}/publications/{publicationIdentifier}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGrantedFundingPublication(string grantedFundingId, string publicationIdentifier)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpPublicApiFundersGrantedFundingPublications.FindAsync(publicationIdentifier);

        if (entity == null)
        {
            return NotFound();
        }

        _importDbContext.ImpPublicApiFundersGrantedFundingPublications.Remove(entity);
        await _importDbContext.SaveChangesAsync();

        return Ok();
    }
}