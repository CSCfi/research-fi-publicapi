namespace ResearchFi.Publication;

/// <summary>
/// Organisaation yksikkö
/// </summary>
public class OrganizationUnit
{
    /// <summary>
    /// Yksikön tunnus
    /// </summary>
    public string? Id { get; set; }
    
    /// <summary>
    /// Yksikön nimi
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Enhetens namn
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// The name of the Unit
    /// </summary>
    public string? NameEn { get; set; }
}