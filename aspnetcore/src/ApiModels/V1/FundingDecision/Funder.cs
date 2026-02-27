namespace ResearchFi.FundingDecision;

/// <summary>
/// Funder
/// </summary>
public class Funder
{
    /// <summary>
    /// The name of the funder in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// The name of the funder in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// The name of the funder in English
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// Persistent identifiers of the funder
    /// </summary>
    public List<PersistentIdentifier>? Pids { get; set; }
}