using Nest;

namespace CSC.PublicApi.Interface.Models.FundingDecision;

public class FundingType
{
    public string? TypeId { get; set; }
    public string? NameFi { get; set; }
    public string? NameSv { get; set; }
    public string? NameEn { get; set; }
}