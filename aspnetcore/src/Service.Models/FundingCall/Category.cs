using Nest;

namespace CSC.PublicApi.Service.Models.FundingCall;

public class Category
{
    /// <summary>
    /// Hakualan koodi
    /// </summary>
    public string? CodeValue { get; set; }

    /// <summary>
    /// Hakuala
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Hakuala
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Hakuala
    /// </summary>
    public string? NameEn { get; set; }
}