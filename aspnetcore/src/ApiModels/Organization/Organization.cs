namespace ResearchFi.Organization;

/// <summary>
/// Organization
/// </summary>
public class Organization
{
    /// <summary>
    /// Name of the Organization in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Name of the Organization in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the Organization in English
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Researchfi portal URL
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }
}