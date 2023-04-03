using ResearchFi.CodeList;

namespace ResearchFi.Publication;

/// <summary>
/// Julkaisun tekijä
/// </summary>
public class Author
{
    /// <summary>
    /// Tekijän etunimet
    /// </summary>
    public string? FirstNames { get; set; }
    
    /// <summary>
    /// Tekijän sukunimi
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Tekijän tutkijatunniste
    /// </summary>
    public string? Orcid { get; set; }
    
    /// <summary>
    /// Tekijän taidejulkaisurooli
    ///
    /// http://uri.suomi.fi/codelist/research/TaidejulkaisuRooli
    /// </summary>
    public ArtPublicationRole? ArtPublicationRole { get; set; }

    /// <summary>
    /// Tekijän organisaatiot
    /// </summary>
    public List<AuthorOrganization>? Organizations { get; set; }
}