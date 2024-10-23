namespace CSC.PublicApi.Service.Models.Publication;

// DTO used when collecting Keywords from OrgPublications (yhteisjulkaisu/osajulkaisu).
public class OrgPublicationKeywordDTO
{
    public string? Id { get; set; }

    public List<Keyword>? Keywords { get; set; }
}