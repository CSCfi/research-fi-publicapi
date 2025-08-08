using CSC.PublicApi.Interface.Models.FundersAPI.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpLinkGrantedFundingToPublication = CSC.PublicApi.Interface.Models.ImportDb.Entities.ImpLinkGrantedFundingToPublication;

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
    /// List publications linked to granted funding.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="organizationId">Organization identifier.</param>
    /// <returns>Collection of <see cref="GrantedFundingToPublication"/></returns>
    [HttpGet("{grantedFundingId}/publications/{organizationId}", Name = "GetGrantedFundingToPublications")]
    [Authorize(Policy = ApiPolicies.Funder.Read)]
    [ProducesResponseType(typeof(IEnumerable<GrantedFundingToPublication>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async IAsyncEnumerable<GrantedFundingToPublication> GetGrantedFundingPublications(string grantedFundingId, string organizationId)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entities = _importDbContext.ImpLinkGrantedFundingToPublications
            .AsNoTracking()
            .Where(e =>
                e.ClientId == clientId &&
                e.FunderProjectNumber == grantedFundingId &&
                e.OrganizationId == organizationId)
            .OrderBy(e => e.OrganizationId);
        if (!entities.Any())
        {
            yield break;
        }

        await foreach (var e in entities.AsAsyncEnumerable())
        {
            yield return new GrantedFundingToPublication
            {
                GrantedFundingId = e.FunderProjectNumber,
                OrganizationId = e.OrganizationId,
                PublicationId = e.PublicationId,
                Doi = e.Doi
            };
        }
    }
    

    /// <summary>
    /// Link publication to granted funding.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="grantedFundingToPublicationModel">Publication details provided in the request body.</param>
    /// <returns><see cref="GrantedFundingPublication"/></returns>
    [HttpPost("{grantedFundingId}/publications/{organizationId}", Name = "PostGrantedFundingToPublication")]
    [Authorize(Policy = ApiPolicies.Funder.Write)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PostGrantedFundingToPublication(string grantedFundingId, string organizationId, [FromBody] PostGrantedFundingToPublication postGrantedFundingToPublicationModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (postGrantedFundingToPublicationModel == null || string.IsNullOrEmpty(postGrantedFundingToPublicationModel.PublicationId) && string.IsNullOrEmpty(postGrantedFundingToPublicationModel.Doi))
        {
            return BadRequest("Either PublicationId or Doi is required");
        }
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        if (
            await _importDbContext.ImpLinkGrantedFundingToPublications
                .AnyAsync(e =>
                e.ClientId == clientId &&
                e.FunderProjectNumber == grantedFundingId &&
                e.OrganizationId == organizationId &&
                e.PublicationId == postGrantedFundingToPublicationModel.PublicationId &&
                e.Doi == postGrantedFundingToPublicationModel.Doi)
        )
        {
            return Conflict("Already exists");
        }

        _importDbContext.ImpLinkGrantedFundingToPublications.Add(
            new ImpLinkGrantedFundingToPublication
            {
                ClientId = clientId,
                OrganizationId = organizationId,
                FunderProjectNumber = grantedFundingId,
                PublicationId = postGrantedFundingToPublicationModel.PublicationId,
                Doi = postGrantedFundingToPublicationModel.Doi,
                Created = DateTime.UtcNow
            }
        );
        await _importDbContext.SaveChangesAsync();
        return CreatedAtAction(
            nameof(GetGrantedFundingToPublication),
            new { grantedFundingId = grantedFundingId, organizationId = organizationId, publicationIdentifier = postGrantedFundingToPublicationModel.GetPublicationIdentifier() },
            new GrantedFundingToPublication
            {
                GrantedFundingId = grantedFundingId,
                OrganizationId = organizationId,
                PublicationId = postGrantedFundingToPublicationModel.PublicationId,
                Doi = postGrantedFundingToPublicationModel.Doi
            }
        );
    }

    
    /// <summary>
    /// Get publication linked to granted funding.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="publicationIdentifier">Publication Identifier.</param>
    /// <returns><see cref="GrantedFundingToPublication"/></returns>
    [HttpGet("{grantedFundingId}/publications/{organizationId}/{publicationIdentifier}", Name = "GetGrantedFundingToPublication")]
    [Authorize(Policy = ApiPolicies.Funder.Read)]
    [ProducesResponseType(typeof(GrantedFundingToPublication), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrantedFundingToPublication(string grantedFundingId, string organizationId, string publicationIdentifier)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpLinkGrantedFundingToPublications
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.ClientId == clientId &&
                e.FunderProjectNumber == grantedFundingId &&
                e.OrganizationId == organizationId &&
                (e.PublicationId == publicationIdentifier || e.Doi == publicationIdentifier));
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(new GrantedFundingToPublication
        {
            GrantedFundingId = entity.FunderProjectNumber,
            OrganizationId = entity.OrganizationId,
            PublicationId = entity.PublicationId,
            Doi = entity.Doi
        });
    }

    /// <summary>
    /// Modify publication linked to granted funding.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="publicationIdentifier">Publication identifier.</param>
    /// <param name="grantedFundingToPublicationModel">Publication details provided in the request body.</param>
    /// <returns><see cref="GrantedFunding"/></returns>
    [HttpPut("{grantedFundingId}/publications/{organizationId}/{publicationIdentifier}", Name = "PutGrantedFundingToPublication")]
    [Authorize(Policy = ApiPolicies.Funder.Write)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutGrantedFundingToPublication(string grantedFundingId, string organizationId, string publicationIdentifier, [FromBody] GrantedFundingToPublication grantedFundingToPublicationModel)
    {
        if (grantedFundingToPublicationModel == null || string.IsNullOrEmpty(grantedFundingToPublicationModel.PublicationId) && string.IsNullOrEmpty(grantedFundingToPublicationModel.Doi))
        {
            return BadRequest("Either PublicationId or Doi is required");
        }
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpLinkGrantedFundingToPublications
            .FirstOrDefaultAsync(e =>
                e.ClientId == clientId &&
                e.OrganizationId == organizationId &&
                e.FunderProjectNumber == grantedFundingId &&
                (e.PublicationId == publicationIdentifier || e.Doi == publicationIdentifier));
        if (entity == null)
        {
            return NotFound();
        }
        entity.FunderProjectNumber = grantedFundingToPublicationModel.GrantedFundingId;
        entity.OrganizationId = grantedFundingToPublicationModel.OrganizationId;
        entity.PublicationId = grantedFundingToPublicationModel.PublicationId;
        entity.Doi = grantedFundingToPublicationModel.Doi;
        entity.Modified = DateTime.UtcNow;
        await _importDbContext.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Delete publication link to granted funding.
    /// </summary>
    /// <param name="grantedFundingId">Granted funding Id.</param>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="publicationIdentifier">Publication identifier.</param>
    [HttpDelete("{grantedFundingId}/publications/{organizationId}/{publicationIdentifier}", Name = "DeleteGrantedFundingToPublication")]
    [Authorize(Policy = ApiPolicies.Funder.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGrantedFundingToPublication(string grantedFundingId, string organizationId, string publicationIdentifier)
    {
        string clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var entity = await _importDbContext.ImpLinkGrantedFundingToPublications
            .FirstOrDefaultAsync(e =>
                e.ClientId == clientId &&
                e.OrganizationId == organizationId &&
                e.FunderProjectNumber == grantedFundingId &&
                (e.PublicationId == publicationIdentifier || e.Doi == publicationIdentifier));
        if (entity == null)
        {
            return NotFound();
        }
        _importDbContext.ImpLinkGrantedFundingToPublications.Remove(entity);
        await _importDbContext.SaveChangesAsync();
        return NoContent();
    }                
}