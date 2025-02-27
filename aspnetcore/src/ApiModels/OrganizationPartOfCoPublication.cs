namespace ResearchFi;

/// <summary>
/// Organization part of co-publication
/// </summary>
public class OrganizationPartOfCoPublication
{
    /// <summary>
    /// Organization part of co-publication - publication ID
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// Organization part of co-publication - organization
    /// </summary>
    public OrganizationPartOfCoPublication_Organization? Organization { get; set; }
}