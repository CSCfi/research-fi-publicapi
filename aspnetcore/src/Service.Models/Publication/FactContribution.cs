namespace CSC.PublicApi.Service.Models.Publication;

public class FactContribution
{
    public int? OrganizationId { get; set; }
    
    public Name? Name { get; set; }

    public ReferenceData? ArtPublicationRole { get; set; }

    public string? ContributionType { get; set; }
}