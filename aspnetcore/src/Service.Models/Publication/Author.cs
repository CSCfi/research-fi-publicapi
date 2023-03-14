using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class Author
{
    public string? FirstNames { get; set; }
    
    public string? LastName { get; set; }

    [Keyword]
    public string? Orcid { get; set; }

    public List<AuthorOrganization>? Organizations { get; set; }
    
    public ReferenceData? ArtPublicationRole { get; set; }
}