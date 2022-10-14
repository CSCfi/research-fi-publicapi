namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrGrantedPermission
{
    public int DimUserProfileId { get; set; }
    public int DimExternalServiceId { get; set; }
    public int DimPermittedFieldGroup { get; set; }

    public virtual DimUserProfile DimUserProfile { get; set; } = null!;
}