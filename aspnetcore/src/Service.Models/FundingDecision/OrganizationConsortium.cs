namespace CSC.PublicApi.Service.Models.FundingDecision;

public class OrganizationConsortium
{
    public Organization Organization { get; set; }
    public string? RoleInConsortium { get; set; }
    public decimal? ShareOfFundingInEur { get; set; }

    public FundingReceiver ToFundingReceiver()
    {
        var receiver =  new FundingReceiver
        {
            Organization = Organization,
            RoleInFundingGroup = RoleInConsortium,
            ShareOfFundingInEur = ShareOfFundingInEur
        };
        
        if (receiver.Organization.Pids != null && !receiver.Organization.Pids.Any())
        {
            receiver.Organization.Pids = null;
        }

        return receiver;
    }
}