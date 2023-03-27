namespace ResearchFi.FundingDecision;

/// <summary>
/// Rahoittaja
/// </summary>
public class Funder
{
    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Finansiärens namn
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// The name of the financier
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// Rahoittajan tunnisteet
    /// </summary>
    public List<PersistentIdentifier>? Ids { get; set; }
}