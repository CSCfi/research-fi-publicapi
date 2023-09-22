namespace ResearchFi.ResearchDataset;

/// <summary>
/// Organization
/// </summary>
public class Organization
{
    /// <summary>
    /// ID of the organization
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Name of the organization in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Name of the organization in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the organization in English
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// Name variants of the organization
    /// </summary>
    public string? NameVariants { get; set; }
}