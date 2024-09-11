using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrGrantedPermission
{
    public int DimUserProfileId { get; set; }

    public int DimExternalServiceId { get; set; }

    public int DimPermittedFieldGroup { get; set; }

    public virtual DimPurpose DimExternalService { get; set; } = null!;

    public virtual DimReferencedatum DimPermittedFieldGroupNavigation { get; set; } = null!;

    public virtual DimUserProfile DimUserProfile { get; set; } = null!;
}
