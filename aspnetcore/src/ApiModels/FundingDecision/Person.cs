namespace ResearchFi.FundingDecision;

/// <summary>
/// Henkilö
/// </summary>
public class Person
{
    /// <summary>
    /// ORCID
    /// </summary>
    public string? OrcId { get; set; }
    
    /// <summary>
    /// Henkilön etunimet
    /// </summary>
    public string? FirstNames { get; set; }
    
    /// <summary>
    /// Henkilön sukunimi
    /// </summary>
    public string? LastName { get; set; }
}