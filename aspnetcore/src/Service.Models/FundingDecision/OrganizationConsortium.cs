namespace CSC.PublicApi.Service.Models.FundingDecision;

public class OrganizationConsortium
{
    public int OrganizationId { get; set; }
    public string? RoleInConsortium { get; set; }
    public decimal? ShareOfFundingInEur { get; set; }
}