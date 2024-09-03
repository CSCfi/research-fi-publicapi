using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimAffiliation
{
    public int Id { get; set; }

    public int DimKnownPersonId { get; set; }

    public int DimOrganizationId { get; set; }

    public int StartDate { get; set; }

    public int? EndDate { get; set; }

    public int? PositionCode { get; set; }

    public string? PositionNameFi { get; set; }

    public string? PositionNameEn { get; set; }

    public string? PositionNameSv { get; set; }

    public string? SourceDescription { get; set; }

    public string SourceId { get; set; } = null!;

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public string? LocalIdentifier { get; set; }

    public string? AffiliationTypeFi { get; set; }

    public string? AffiliationTypeEn { get; set; }

    public string? AffiliationTypeSv { get; set; }

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;

    public virtual DimOrganization DimOrganization { get; set; } = null!;

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimDate? EndDateNavigation { get; set; }

    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; } = new List<FactFieldValue>();

    public virtual DimReferencedatum? PositionCodeNavigation { get; set; }

    public virtual DimDate StartDateNavigation { get; set; } = null!;
}
