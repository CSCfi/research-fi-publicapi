namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrServiceSubscription
{
    public int DimUserProfileId { get; set; }
    public int DimExternalServiceId { get; set; }

    public virtual DimExternalService DimExternalService { get; set; } = null!;
    public virtual DimUserProfile DimUserProfile { get; set; } = null!;
}