namespace CSC.PublicApi.Service.Models.Publication;

// DTO used when collecting fields of art from OrgPublications (yhteisjulkaisu/osajulkaisu).
public class OrgPublicationFieldsOfArtDTO
{
    public string? Id { get; set; }

    public List<ReferenceData>? FieldsOfArt { get; set; }
}