namespace ResearchFi.FundingDecision;

/// <summary>
/// Organisaatio
/// </summary>
public class OrganizationConsortium
{
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
    /// Organisaation tunnukset
    /// </summary>
    public List<PersistentIdentifier>? Ids { get; set; }
    
    /// <summary>
    /// Rooli EU:n hankkeessa
    /// </summary>
    public string? RoleInConsortium { get; set; }
    
    /// <summary>
    /// Organisaatiolle myönnetty osuus myöntösummasta
    /// </summary>
    public decimal? ShareOfFundingInEur { get; set; }
    
    /// <summary>
    /// Suomalainen organisaatio
    /// </summary>
    public bool? IsFinnishOrganization { get; set; }
}