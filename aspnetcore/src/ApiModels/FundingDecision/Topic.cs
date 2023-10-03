namespace ResearchFi.FundingDecision;

/// <summary>
/// Topic of the funding call
/// </summary>
public class Topic
{
    /// <summary>
    /// Topic ID of the funding call
    /// </summary>
    public string? TopicId { get; set; }
    
    /// <summary>
    /// Name of the funding call in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Name of the funding call in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the funding call in English
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// European union call programme ID
    /// </summary>
    public string? EuCallId { get; set; }
}