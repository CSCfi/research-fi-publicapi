namespace ResearchFi.FundingDecision;

/// <summary>
/// Part of the program
/// </summary>
public class CallProgramme
{
    /// <summary>
    /// ID of the part of the program
    /// </summary>
    public string? CallProgrammeId { get; set; }
    
    /// <summary>
    /// The topic of the program in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// The topic of the program in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// The topic of the program in English
    /// </summary>
    public string? NameEn { get; set; }
}