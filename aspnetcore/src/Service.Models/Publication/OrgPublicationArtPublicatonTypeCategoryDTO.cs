namespace CSC.PublicApi.Service.Models.Publication;

// DTO used when collecting ArtPublicationTypeCategories from OrgPublications (yhteisjulkaisu/osajulkaisu).
public class OrgPublicationArtPublicatonTypeCategoryDTO
{
    public string? Id { get; set; }

    public List<ReferenceData>? ArtPublicationTypeCategories { get; set; }
}