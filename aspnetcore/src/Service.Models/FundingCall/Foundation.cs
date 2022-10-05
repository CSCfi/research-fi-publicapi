using Nest;

namespace CSC.PublicApi.Service.Models.FundingCall;

public class Foundation
{
    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    [Text(Name = "nameFi")]
    public string? NameFi { get; set; }

    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    [Text(Name = "nameSv")]
    public string? NameSv { get; set; }

    /// <summary>
    /// Rahoittajan nimi
    /// </summary>
    [Text(Name = "nameEn")]
    public string? NameEn { get; set; }

    /// <summary>
    /// Y-tunnus
    /// </summary>
    [Text(Name = "businessId")]
    public string? BusinessId { get; set; }

    /// <summary>
    /// Rahoittajan verkkosivu
    /// </summary>
    [Text(Name = "foundationURL")]
    public string? FoundationUrl { get; set; }
}