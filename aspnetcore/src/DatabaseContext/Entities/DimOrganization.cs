﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimOrganization
{
    public int Id { get; set; }

    public int? DimOrganizationBroader { get; set; }

    public int DimSectorid { get; set; }

    public string? OrganizationId { get; set; }

    public bool? OrganizationActive { get; set; }

    public string? LocalOrganizationUnitId { get; set; }

    public string? LocalOrganizationSector { get; set; }

    public string? OrganizationBackground { get; set; }

    public string? OrganizationType { get; set; }

    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }

    public string? NameVariants { get; set; }

    public string? NameUnd { get; set; }

    public string? CountryCode { get; set; }

    public DateTime? Established { get; set; }

    public string? VisitingAddress { get; set; }

    public string? PostalAddress { get; set; }

    public int? StaffCountAsFte { get; set; }

    public int? DegreeCountBsc { get; set; }

    public int? DegreeCountMsc { get; set; }

    public int? DegreeCountLic { get; set; }

    public int? DegreeCountPhd { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public virtual ICollection<BrFundingConsortiumParticipation> BrFundingConsortiumParticipations { get; set; } = new List<BrFundingConsortiumParticipation>();

    public virtual ICollection<BrParticipatesInFundingGroup> BrParticipatesInFundingGroups { get; set; } = new List<BrParticipatesInFundingGroup>();

    public virtual ICollection<DimAffiliation> DimAffiliations { get; set; } = new List<DimAffiliation>();

    public virtual ICollection<DimExternalService> DimExternalServices { get; set; } = new List<DimExternalService>();

    public virtual ICollection<DimFundingDecision> DimFundingDecisions { get; set; } = new List<DimFundingDecision>();

    public virtual DimOrganization? DimOrganizationBroaderNavigation { get; set; }

    public virtual ICollection<DimPid> DimPids { get; set; } = new List<DimPid>();

    public virtual ICollection<DimProfileOnlyFundingDecision> DimProfileOnlyFundingDecisions { get; set; } = new List<DimProfileOnlyFundingDecision>();

    public virtual ICollection<DimProfileOnlyResearchActivity> DimProfileOnlyResearchActivities { get; set; } = new List<DimProfileOnlyResearchActivity>();

    public virtual ICollection<DimPurpose> DimPurposes { get; set; } = new List<DimPurpose>();

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual ICollection<DimRegisteredDataSource> DimRegisteredDataSources { get; set; } = new List<DimRegisteredDataSource>();

    public virtual ICollection<DimResearchActivity> DimResearchActivities { get; set; } = new List<DimResearchActivity>();

    public virtual ICollection<DimResearchProject> DimResearchProjects { get; set; } = new List<DimResearchProject>();

    public virtual DimSector DimSector { get; set; } = null!;

    public virtual ICollection<DimWebLink> DimWebLinks { get; set; } = new List<DimWebLink>();

    public virtual ICollection<FactContribution> FactContributions { get; set; } = new List<FactContribution>();

    public virtual ICollection<FactUpkeep> FactUpkeeps { get; set; } = new List<FactUpkeep>();

    public virtual ICollection<DimOrganization> InverseDimOrganizationBroaderNavigation { get; set; } = new List<DimOrganization>();

    public virtual ICollection<DimCallProgramme> DimCallProgrammes { get; set; } = new List<DimCallProgramme>();

    public virtual ICollection<DimOrganization> DimOrganizationid2s { get; set; } = new List<DimOrganization>();

    public virtual ICollection<DimOrganization> DimOrganizationid2sNavigation { get; set; } = new List<DimOrganization>();

    public virtual ICollection<DimOrganization> DimOrganizations { get; set; } = new List<DimOrganization>();

    public virtual ICollection<DimOrganization> DimOrganizationsNavigation { get; set; } = new List<DimOrganization>();
}
