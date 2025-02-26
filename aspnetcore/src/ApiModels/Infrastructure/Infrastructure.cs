namespace ResearchFi.Infrastructure;

/// <summary>
/// Infrastructure
/// </summary>
public class Infrastructure
{
    /// <summary>
    /// Name of the infrastructure in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Name of the infrastructure in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the infrastructure in English
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Researchfi portal URL
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }
}