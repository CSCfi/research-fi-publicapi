namespace ResearchFi.FundingDecision;

/// <summary>
/// Organization
/// </summary>
public class Organization
{
    /// <summary>
    /// Persistent identifiers of the organization
    /// </summary>
    public List<PersistentIdentifier>? Pids { get; set; }

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
    /// Coountry code
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Is the organization Finnish
    /// </summary>
    public bool IsFinnishOrganization { get; set; }
}