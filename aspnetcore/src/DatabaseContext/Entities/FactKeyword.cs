using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactKeyword
{
    public int DimKeywordId { get; set; }

    public int DimResearchProjectId { get; set; }

    public int DimInfrastructureId { get; set; }

    public int DimResearchDatasetId { get; set; }

    public string SourceId { get; set; } = null!;

    public string SourceDescription { get; set; } = null!;

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;

    public virtual DimKeyword DimKeyword { get; set; } = null!;

    public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;

    public virtual DimResearchProject DimResearchProject { get; set; } = null!;
}
