namespace ResearchFi.FundingDecision;

/// <summary>
/// Rahoitushaun aihe
/// </summary>
public class Topic
{
    /// <summary>
    /// Rahoitushaun aiheen tunnus
    /// </summary>
    public string? TopicId { get; set; }
    
    /// <summary>
    /// Rahoitushaun aihe
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Ämnet för ansökan om finansiering
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// The subject of the funding call
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// Rahoitushaun tunniste
    /// </summary>
    public string? EuCallId { get; set; }
}