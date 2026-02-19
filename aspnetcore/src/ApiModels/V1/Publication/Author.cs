using ResearchFi.CodeList;

namespace ResearchFi.Publication;

/// <summary>
/// Publication author
/// </summary>
public class Author
{
    /// <summary>
    /// First names of the author
    /// </summary>
    public string? FirstNames { get; set; }
    
    /// <summary>
    /// Last name of the author
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// ORCID of the author
    /// </summary>
    public string? Orcid { get; set; }
    
    /// <summary>
    /// Art publications role code of the author
    ///
    /// https://uri.suomi.fi/codelist/research/TaidejulkaisuRooli
    /// </summary>
    public ArtPublicationRole? ArtPublicationRole { get; set; }

    /// <summary>
    /// Organizations of the author
    /// </summary>
    public List<AuthorOrganization>? Organizations { get; set; }
}