namespace ResearchFi;

/// <summary>
/// Pysyvä tunniste
/// </summary>
public class PersistentIdentifier
{
    /// <summary>
    /// Tunniste
    /// </summary>
    public string? Content { get; set; }
    
    /// <summary>
    /// Tunnisteen tyyppi
    /// </summary>
    public string? Type { get; set; }
}