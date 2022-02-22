using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimKeyword
    {
        public DimKeyword()
        {
            BrKeywordDimKeywordDimKeywordId2Navigations = new HashSet<BrKeywordDimKeyword>();
            BrKeywordDimKeywordDimKeywords = new HashSet<BrKeywordDimKeyword>();
            FactFieldValues = new HashSet<FactFieldValue>();
            FactInfraKeywords = new HashSet<FactInfraKeyword>();
            DimFundingDecisions = new HashSet<DimFundingDecision>();
            DimPublications = new HashSet<DimPublication>();
            DimResearchActivities = new HashSet<DimResearchActivity>();
            DimResearchDatasets = new HashSet<DimResearchDataset>();
        }

        public int Id { get; set; }
        public string Keyword { get; set; } = null!;
        public string? Scheme { get; set; }
        public string? ConceptUri { get; set; }
        public string? SchemeUri { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int DimRegisteredDataSourceId { get; set; }
        public int DimReferencedataIdLanguageCode { get; set; }

        public virtual DimReferencedatum DimReferencedataIdLanguageCodeNavigation { get; set; } = null!;
        public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
        public virtual ICollection<BrKeywordDimKeyword> BrKeywordDimKeywordDimKeywordId2Navigations { get; set; }
        public virtual ICollection<BrKeywordDimKeyword> BrKeywordDimKeywordDimKeywords { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
        public virtual ICollection<FactInfraKeyword> FactInfraKeywords { get; set; }

        public virtual ICollection<DimFundingDecision> DimFundingDecisions { get; set; }
        public virtual ICollection<DimPublication> DimPublications { get; set; }
        public virtual ICollection<DimResearchActivity> DimResearchActivities { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasets { get; set; }
    }
}
