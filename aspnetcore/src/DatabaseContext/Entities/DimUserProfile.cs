﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimUserProfile
{
    public int Id { get; set; }

    public int DimKnownPersonId { get; set; }

    public bool AllowAllSubscriptions { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public string? OrcidAccessToken { get; set; }

    public string? OrcidRefreshToken { get; set; }

    public string? OrcidTokenScope { get; set; }

    public DateTime? OrcidTokenExpires { get; set; }

    public string? OrcidId { get; set; }

    public int? Statuscode { get; set; }

    public bool Hidden { get; set; }

    public bool PublishNewOrcidData { get; set; }

    public virtual ICollection<BrGrantedPermission> BrGrantedPermissions { get; set; } = new List<BrGrantedPermission>();

    public virtual ICollection<DimFieldDisplaySetting> DimFieldDisplaySettings { get; set; } = new List<DimFieldDisplaySetting>();

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;

    public virtual ICollection<DimUserChoice> DimUserChoices { get; set; } = new List<DimUserChoice>();

    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; } = new List<FactFieldValue>();
}
