namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrParticipatesInFundingGroup
{
    public int DimFundingDecisionid { get; set; }
    public int DimNameId { get; set; }
    public int DimOrganizationId { get; set; }
    public string? RoleInFundingGroup { get; set; }
    public decimal? ShareOfFundingInEur { get; set; }
    public string? SourceId { get; set; }
    public bool? EndOfParticipation { get; set; }

    public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;
    public virtual DimName DimName { get; set; } = null!;
    public virtual DimOrganization DimOrganization { get; set; } = null!;
}