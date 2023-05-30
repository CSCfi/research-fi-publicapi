using Nest;

namespace CSC.PublicApi.Service.Models.FundingDecision;

public class FundingReceiver
{
    public Person? Person { get; set; }

    public Organization? Organization { get; set; }
    
    public string? RoleInFundingGroup { get; set; }

    [Number(NumberType.ScaledFloat, ScalingFactor = 100)]
    public decimal? ShareOfFundingInEur { get; set; }
}