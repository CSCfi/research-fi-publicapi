namespace CSC.PublicApi.Service.Models.FundingDecision;

public class FundingGroupPerson
{
    public string SourceId { get; set; }

    public Person? Person { get; set; }

    public string? RoleInFundingGroup { get; set; }

    public Organization Organization { get; set; }

    public decimal ShareOfFundingInEur { get; set; }

    public FundingReceiver ToFundingReceiver()
    {
        var receiver = new FundingReceiver
        {
            Person = Person,
            Organization = Organization,
            RoleInFundingGroup = RoleInFundingGroup,
            ShareOfFundingInEur = ShareOfFundingInEur
        };

        if (receiver.Organization.Pids != null && !receiver.Organization.Pids.Any())
        {
            receiver.Organization.Pids = null;
        }

        return receiver;
    }
}