using Nest;

namespace CSC.PublicApi.Service.Models.FundingCall;

public class Foundation
{
    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Y-tunnus
    /// </summary>
    public string? BusinessId { get; set; }

    /// <summary>
    /// Rahoittajan verkkosivu
    /// </summary>
    public string? FoundationUrl { get; set; }
}