using Nest;

namespace CSC.PublicApi.Service.Models.FundingDecision;

public class OrganizationConsortium
{
    public string? NameFi { get; set; }
    
    public string? NameSv { get; set; }
    
    public string? NameEn { get; set; }

    public List<Id>? Ids { get; set; }

    public string? RoleInConsortium { get; set; }

    [Number(NumberType.ScaledFloat, ScalingFactor = 100)]
    public decimal? ShareOfFundingInEur { get; set; }
    
    public bool? IsFinnishOrganization { get; set; }

}