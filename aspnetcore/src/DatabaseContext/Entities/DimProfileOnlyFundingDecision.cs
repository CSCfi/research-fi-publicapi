﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimProfileOnlyFundingDecision
{
    public int Id { get; set; }

    public int DimDateIdApproval { get; set; }

    public int DimDateIdStart { get; set; }

    public int DimDateIdEnd { get; set; }

    public int DimCallProgrammeId { get; set; }

    public int DimTypeOfFundingId { get; set; }

    public int? DimOrganizationIdFunder { get; set; }

    public string? OrcidWorkType { get; set; }

    public string? FunderProjectNumber { get; set; }

    public string? Acronym { get; set; }

    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }

    public string? NameUnd { get; set; }

    public string? DescriptionFi { get; set; }

    public string? DescriptionEn { get; set; }

    public string? DescriptionSv { get; set; }

    public decimal AmountInEur { get; set; }

    public decimal? AmountInFundingDecisionCurrency { get; set; }

    public string? FundingDecisionCurrencyAbbreviation { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public virtual DimCallProgramme DimCallProgramme { get; set; } = null!;

    public virtual DimDate DimDateIdApprovalNavigation { get; set; } = null!;

    public virtual DimDate DimDateIdEndNavigation { get; set; } = null!;

    public virtual DimDate DimDateIdStartNavigation { get; set; } = null!;

    public virtual DimOrganization? DimOrganizationIdFunderNavigation { get; set; }

    public virtual ICollection<DimPid> DimPids { get; set; } = new List<DimPid>();

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimTypeOfFunding DimTypeOfFunding { get; set; } = null!;

    public virtual ICollection<DimWebLink> DimWebLinks { get; set; } = new List<DimWebLink>();

    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; } = new List<FactFieldValue>();
}
