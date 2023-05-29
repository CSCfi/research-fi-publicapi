namespace CSC.PublicApi.Service.Models.FundingDecision;

public class FundingGroupPerson
{
    public string SourceId { get; set; }

    public Person? Person { get; set; }

    public string? RoleInFundingGroup { get; set; }

    public int OrganizationId { get; set; }

    public decimal ShareOfFundingInEur { get; set; }
}