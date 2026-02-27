namespace ResearchFi.FundingCall;

/// <summary>
/// Funding foundation
/// </summary>
public class Foundation
{
    /// <summary>
    /// Name of the funder in Finnish
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Name of the funder in Swedish
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Name of the funder in English
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Finnish business ID
    /// </summary>
    public string? BusinessId { get; set; }

    /// <summary>
    /// Funders webpage
    /// </summary>
    public string? FoundationUrl { get; set; }
}