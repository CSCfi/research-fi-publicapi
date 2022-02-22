using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class BrWordClusterDimFundingDecision
    {
        public int DimWordClusterId { get; set; }
        public int DimFundingDecisionId { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;
        public virtual DimWordCluster DimWordCluster { get; set; } = null!;
    }
}
