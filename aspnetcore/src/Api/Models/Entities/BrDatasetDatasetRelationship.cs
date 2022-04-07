using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class BrDatasetDatasetRelationship
    {
        public int DimResearchDatasetId { get; set; }
        public int DimResearchDatasetId2 { get; set; }
        public string? Type { get; set; }

        public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;
        public virtual DimResearchDataset DimResearchDatasetId2Navigation { get; set; } = null!;
    }
}
