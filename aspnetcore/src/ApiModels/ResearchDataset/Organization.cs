namespace ResearchFi.ResearchDataset;

/// <summary>
/// Organisaatio
/// </summary>
public class Organization
{
    /// <summary>
    /// Organisaation tunniste
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Organisaation nimi
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Organisations namn
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// The name of the organization
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// Vaihtoehtoiset nimet
    /// </summary>
    public string? NameVariants { get; set; }
}