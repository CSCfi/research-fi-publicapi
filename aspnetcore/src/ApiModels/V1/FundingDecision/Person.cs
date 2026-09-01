namespace ResearchFi.FundingDecision;

/// <summary>
/// Person
/// </summary>
public class Person
{
    /// <summary>
    /// ORCID
    /// </summary>
    public string? OrcId { get; set; }
    
    /// <summary>
    /// First names of the person
    /// </summary>
    public string? FirstNames { get; set; }
    
    /// <summary>
    /// Last name of the person
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// True if this person is the directly funded party in the funding decision
    /// </summary>
    public bool FundedPerson { get; set; }
}