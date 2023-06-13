namespace ResearchFi.FundingDecision;

/// <summary>
/// Organisaatio
/// </summary>
public class Organization
{
    /// <summary>
    /// Organisaation pysyvät tunnisteet
    /// </summary>
    public List<PersistentIdentifier>? Pids { get; set; }

    /// <summary>
    /// Organisaation nimi
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// The name of the organization
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Organisations namn
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Organisaation maakoodi
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Suomalainen organisaatio
    /// </summary>
    public bool IsFinnishOrganization { get; set; }
}