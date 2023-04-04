namespace ResearchFi.FundingDecision;

/// <summary>
/// Ohjelman osa
/// </summary>
public class CallProgramme
{
    /// <summary>
    /// Ohjelman osan tunniste
    /// </summary>
    public string? CallProgrammeId { get; set; }
    
    /// <summary>
    /// Rahoitushaun aihe
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Föremålet för finansieringsansökan
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// The topic of the funding call
    /// </summary>
    public string? NameEn { get; set; }
}