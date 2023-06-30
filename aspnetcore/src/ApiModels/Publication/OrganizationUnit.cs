namespace ResearchFi.Publication;

/// <summary>
/// Organization unit
/// </summary>
public class OrganizationUnit
{
    /// <summary>
    /// ID of the unit
    /// </summary>
    public string? Id { get; set; }
    
    /// <summary>
    /// Name of the unit in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Name of the unit in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the unit in English
    /// </summary>
    public string? NameEn { get; set; }
}