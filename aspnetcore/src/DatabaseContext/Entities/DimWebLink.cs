﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimWebLink
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public string? LinkLabel { get; set; }

    public string? LinkType { get; set; }

    public string? LanguageVariant { get; set; }

    public int? DimOrganizationId { get; set; }

    public int? DimKnownPersonId { get; set; }

    public int? DimCallProgrammeId { get; set; }

    public int? DimFundingDecisionId { get; set; }

    public int? DimResearchDataCatalogId { get; set; }

    public int? DimResearchDatasetId { get; set; }

    public int? DimResearchCommunityId { get; set; }

    public string? SourceDescription { get; set; }

    public string SourceId { get; set; } = null!;

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int? DimResearchActivityId { get; set; }

    public int? DimRegisteredDataSourceId { get; set; }

    public int? DimProfileOnlyResearchActivityId { get; set; }

    public int? DimProfileOnlyDatasetId { get; set; }

    public int? DimProfileOnlyFundingDecisionId { get; set; }

    public int? DimResearchProjectId { get; set; }

    public int? DimServiceId { get; set; }

    public int? DimInfrastructureId { get; set; }

    public virtual DimCallProgramme? DimCallProgramme { get; set; }

    public virtual DimFundingDecision? DimFundingDecision { get; set; }

    public virtual DimInfrastructure? DimInfrastructure { get; set; }

    public virtual DimKnownPerson? DimKnownPerson { get; set; }

    public virtual DimOrganization? DimOrganization { get; set; }

    public virtual DimProfileOnlyDataset? DimProfileOnlyDataset { get; set; }

    public virtual DimProfileOnlyFundingDecision? DimProfileOnlyFundingDecision { get; set; }

    public virtual DimProfileOnlyResearchActivity? DimProfileOnlyResearchActivity { get; set; }

    public virtual DimRegisteredDataSource? DimRegisteredDataSource { get; set; }

    public virtual DimResearchActivity? DimResearchActivity { get; set; }

    public virtual DimResearchCommunity? DimResearchCommunity { get; set; }

    public virtual DimResearchDataCatalog? DimResearchDataCatalog { get; set; }

    public virtual DimResearchDataset? DimResearchDataset { get; set; }

    public virtual DimService? DimService { get; set; }

    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; } = new List<FactFieldValue>();
}
