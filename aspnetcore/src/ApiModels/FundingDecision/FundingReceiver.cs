namespace ResearchFi.FundingDecision;

/// <summary>
/// Rahoituksen saaja
/// </summary>
public class FundingReceiver
{
    /// <summary>
    /// Rahoituksen saanut henkilö
    /// </summary>
    public Person? Person { get; set; }

    /// <summary>
    /// Rahoituksen saanut organisaatio
    /// </summary>
    public Organization Organization { get; set; }
    
    /// <summary>
    /// Rahoituspäätöksen numero
    /// </summary>
    public string? FunderProjectNumber { get; set; }
    
    /// <summary>
    /// Rooli rahoitusryhmässä
    /// </summary>
    public string? RoleInFundingGroup { get; set; }

    /// <summary>
    /// Rahoituksen osuus
    /// </summary>
    public decimal? ShareOfFundingInEur { get; set; }
}