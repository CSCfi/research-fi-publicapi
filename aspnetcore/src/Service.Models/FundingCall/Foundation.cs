using Nest;

namespace CSC.PublicApi.Service.Models.FundingCall;

public class Foundation
{
    [Ignore]
    public int Id { get; set; }

    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    [Text]
    public string? NameFi { get; set; }

    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    [Text]
    public string? NameSv { get; set; }

    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    [Text]
    public string? NameEn { get; set; }

    /// <summary>
    /// Y-tunnus
    /// </summary>
    [Keyword]
    public string? BusinessId { get; set; }

    /// <summary>
    /// Rahoittajan verkkosivu
    /// </summary>
    [Ignore]
    public string? FoundationUrl { get; set; }
}