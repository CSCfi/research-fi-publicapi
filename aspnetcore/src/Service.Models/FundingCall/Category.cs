using Nest;

namespace CSC.PublicApi.Service.Models.FundingCall;

public class Category
{
    /// <summary>
    /// Hakualan koodi
    /// </summary>
    [Text(Name = "codeValue")]
    public string? CodeValue { get; set; }

    /// <summary>
    /// Hakuala
    /// </summary>
    [Text(Name = "nameFi")]
    public string? NameFi { get; set; }

    /// <summary>
    /// Hakuala
    /// </summary>
    [Text(Name = "nameSv")]
    public string? NameSv { get; set; }

    /// <summary>
    /// Hakuala
    /// </summary>
    [Text(Name = "nameEn")]
    public string? NameEn { get; set; }
}