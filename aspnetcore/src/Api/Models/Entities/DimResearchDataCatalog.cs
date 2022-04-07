﻿using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimResearchDataCatalog
    {
        public DimResearchDataCatalog()
        {
            DimPids = new HashSet<DimPid>();
            DimResearchDatasets = new HashSet<DimResearchDataset>();
            FactContributions = new HashSet<FactContribution>();
        }

        public int Id { get; set; }
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionFi { get; set; }
        public string? DescriptionSv { get; set; }
        public string? DescriptionEn { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ICollection<DimPid> DimPids { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasets { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
    }
}
