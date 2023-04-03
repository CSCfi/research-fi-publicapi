namespace ResearchFi.FundingCall;

/// <summary>
/// Rahoittaja
/// </summary>
public class Foundation
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
    /// Y-tunnus
    /// </summary>
    public string? BusinessId { get; set; }

    /// <summary>
    /// Rahoittajan verkkosivu
    /// </summary>
    public string? FoundationUrl { get; set; }
}