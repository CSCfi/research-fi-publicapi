﻿using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimGeo
    {
        public DimGeo()
        {
            DimEvents = new HashSet<DimEvent>();
            DimFundingDecisions = new HashSet<DimFundingDecision>();
            DimResearchActivities = new HashSet<DimResearchActivity>();
            FactContributions = new HashSet<FactContribution>();
            FactUpkeeps = new HashSet<FactUpkeep>();
        }

        public int Id { get; set; }
        public string? CountryCode { get; set; }
        public string CountryId { get; set; } = null!;
        public string? RegionId { get; set; }
        public string? MunicipalityId { get; set; }
        public string CountryFi { get; set; } = null!;
        public string? CountryEn { get; set; }
        public string? CountrySv { get; set; }
        public string? RegionFi { get; set; }
        public string? RegionSv { get; set; }
        public string? MunicipalityFi { get; set; }
        public string? MunicipalitySv { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ICollection<DimEvent> DimEvents { get; set; }
        public virtual ICollection<DimFundingDecision> DimFundingDecisions { get; set; }
        public virtual ICollection<DimResearchActivity> DimResearchActivities { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactUpkeep> FactUpkeeps { get; set; }
    }
}
