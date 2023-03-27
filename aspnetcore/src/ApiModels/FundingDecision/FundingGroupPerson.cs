namespace ResearchFi.FundingDecision;

/// <summary>
/// Rahoituksen saaja
/// </summary>
public class FundingGroupPerson
{
    /// <summary>
    /// ORCID
    /// </summary>
    public string? OrcId { get; set; }
    
    /// <summary>
    /// Saajan etunimet
    /// </summary>
    public string? FirstNames { get; set; }
    
    /// <summary>
    /// Saajan sukunimi
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Rooli Akatemian konsortiossa
    /// </summary>
    public string? RoleInFundingGroup { get; set; }
}