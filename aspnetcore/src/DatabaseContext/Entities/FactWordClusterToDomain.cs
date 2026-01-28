using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactWordClusterToDomain
{
    public int DimWordClusterId { get; set; }

    public int DimFundingDecisionId { get; set; }

    public int DimPublicationId { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;

    public virtual DimPublication DimPublication { get; set; } = null!;

    public virtual DimWordCluster DimWordCluster { get; set; } = null!;
}
