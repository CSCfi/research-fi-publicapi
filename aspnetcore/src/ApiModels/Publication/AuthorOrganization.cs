namespace ResearchFi.Publication;

/// <summary>
/// Julkaisun tekijän organisaatio
/// </summary>
public class AuthorOrganization
{
    /// <summary>
    /// Organisaation tunnus
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Organisaation yksiköt
    /// </summary>
    public List<string>? UnitIds { get; set; }
}