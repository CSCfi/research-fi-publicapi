namespace ResearchFi.FundingDecision;

/// <summary>
/// Receiver of funding
/// </summary>
public class FundingReceiver
{
    /// <summary>
    /// Funding receiving person
    /// </summary>
    public Person? Person { get; set; }

    /// <summary>
    /// Funding receiving organization 
    /// </summary>
    public Organization Organization { get; set; }
    
    /// <summary>
    /// Funder project number
    /// </summary>
    public string? FunderProjectNumber { get; set; }
    
    /// <summary>
    /// Role in funding group
    /// </summary>
    public string? RoleInFundingGroup { get; set; }

    /// <summary>
    /// Share of funding in euros
    /// </summary>
    public decimal? ShareOfFundingInEur { get; set; }
}