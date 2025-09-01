using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactConceptContext
{
    public int DimConceptId { get; set; }

    public int DimPublicationId { get; set; }

    public int DimFundingDecisionId { get; set; }

    public int DimResearchActivityId { get; set; }

    public int DimProfileOnlyPublicationId { get; set; }

    public int DimResearchDatasetId { get; set; }

    public int? DimInfrastructureId { get; set; }

    public int DimServiceId { get; set; }

    public string? SourceDescription { get; set; }

    public string SourceId { get; set; } = null!;

    public DateTime? Modified { get; set; }

    public DateTime? Created { get; set; }

    public virtual DimConcept DimConcept { get; set; } = null!;

    public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;

    public virtual DimInfrastructure? DimInfrastructure { get; set; }

    public virtual DimProfileOnlyPublication DimProfileOnlyPublication { get; set; } = null!;

    public virtual DimPublication DimPublication { get; set; } = null!;

    public virtual DimResearchActivity DimResearchActivity { get; set; } = null!;

    public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;
}
