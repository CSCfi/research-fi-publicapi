namespace CSC.PublicApi.Service.Models.Publication;

// DTO used when collecting fields of science from OrgPublications (yhteisjulkaisu/osajulkaisu).
public class OrgPublicationFieldsOfScienceDTO
{
    public string? Id { get; set; }

    public List<ReferenceData>? FieldsOfScience { get; set; }
}