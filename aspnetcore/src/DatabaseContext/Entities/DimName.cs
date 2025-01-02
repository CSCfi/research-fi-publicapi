﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimName
{
    public long Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstNames { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimKnownPersonIdConfirmedIdentity { get; set; }

    public string? SourceProjectId { get; set; }

    public string? FullName { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public virtual ICollection<BrParticipatesInFundingGroup> BrParticipatesInFundingGroups { get; set; } = new List<BrParticipatesInFundingGroup>();

    public virtual ICollection<DimFundingDecision> DimFundingDecisions { get; set; } = new List<DimFundingDecision>();

    public virtual DimKnownPerson DimKnownPersonIdConfirmedIdentityNavigation { get; set; } = null!;

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual ICollection<DimResearchProject> DimResearchProjects { get; set; } = new List<DimResearchProject>();

    public virtual ICollection<FactContribution> FactContributions { get; set; } = new List<FactContribution>();

    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; } = new List<FactFieldValue>();
}
