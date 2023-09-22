namespace ResearchFi.Publication;

/// <summary>
/// Organization of the author of the publication
/// </summary>
public class AuthorOrganization
{
    /// <summary>
    /// Organization ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Unit IDs of the organization
    /// </summary>
    public List<string>? UnitIds { get; set; }
}