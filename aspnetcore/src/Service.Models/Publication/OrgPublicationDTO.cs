namespace CSC.PublicApi.Service.Models.Publication;

// DTO used when collecting DatabaseContributions from OrgPublications (yhteisjulkaisu/osajulkaisu).
public class OrgPublicationDTO
{
    public string? Id { get; set; }

    public List<FactContribution>? DatabaseContributions { get; set; }
}