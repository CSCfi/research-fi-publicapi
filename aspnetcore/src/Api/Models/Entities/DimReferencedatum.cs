using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimReferencedatum
    {
        public DimReferencedatum()
        {
            BrKeywordDimKeywords = new HashSet<BrKeywordDimKeyword>();
            DimAffiliationAffiliationTypeNavigations = new HashSet<DimAffiliation>();
            DimAffiliationPositionCodeNavigations = new HashSet<DimAffiliation>();
            DimEducations = new HashSet<DimEducation>();
            DimKeywords = new HashSet<DimKeyword>();
            DimNewsFeeds = new HashSet<DimNewsFeed>();
            DimOrcidPublicationArticleTypeCodeNavigations = new HashSet<DimOrcidPublication>();
            DimOrcidPublicationDimReferencedata = new HashSet<DimOrcidPublication>();
            DimOrcidPublicationParentPublicationTypeCodeNavigations = new HashSet<DimOrcidPublication>();
            DimOrcidPublicationPublicationTypeCode2Navigations = new HashSet<DimOrcidPublication>();
            DimOrcidPublicationPublicationTypeCodeNavigations = new HashSet<DimOrcidPublication>();
            DimOrcidPublicationTargetAudienceCodeNavigations = new HashSet<DimOrcidPublication>();
            DimPublicationArticleTypeCodeNavigations = new HashSet<DimPublication>();
            DimPublicationParentPublicationTypeCodeNavigations = new HashSet<DimPublication>();
            DimPublicationPublicationTypeCode2Navigations = new HashSet<DimPublication>();
            DimPublicationTargetAudienceCodeNavigations = new HashSet<DimPublication>();
            DimResearchDatasets1 = new HashSet<DimResearchDataset>();
            DimUserChoices = new HashSet<DimUserChoice>();
            FactContributions = new HashSet<FactContribution>();
            FactJufoClassCodesForPubChannels = new HashSet<FactJufoClassCodesForPubChannel>();
            DimCallProgrammes = new HashSet<DimCallProgramme>();
            DimResearchDatasets = new HashSet<DimResearchDataset>();
            DimResearchDatasetsNavigation = new HashSet<DimResearchDataset>();
        }

        public int Id { get; set; }
        public string CodeScheme { get; set; } = null!;
        public string CodeValue { get; set; } = null!;
        public int? Order { get; set; }
        public string? State { get; set; }
        public string? NameFi { get; set; }
        public string? NameEn { get; set; }
        public string? NameSv { get; set; }
        public string SourceId { get; set; } = null!;
        public string SourceDescription { get; set; } = null!;
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ICollection<BrKeywordDimKeyword> BrKeywordDimKeywords { get; set; }
        public virtual ICollection<DimAffiliation> DimAffiliationAffiliationTypeNavigations { get; set; }
        public virtual ICollection<DimAffiliation> DimAffiliationPositionCodeNavigations { get; set; }
        public virtual ICollection<DimEducation> DimEducations { get; set; }
        public virtual ICollection<DimKeyword> DimKeywords { get; set; }
        public virtual ICollection<DimNewsFeed> DimNewsFeeds { get; set; }
        public virtual ICollection<DimOrcidPublication> DimOrcidPublicationArticleTypeCodeNavigations { get; set; }
        public virtual ICollection<DimOrcidPublication> DimOrcidPublicationDimReferencedata { get; set; }
        public virtual ICollection<DimOrcidPublication> DimOrcidPublicationParentPublicationTypeCodeNavigations { get; set; }
        public virtual ICollection<DimOrcidPublication> DimOrcidPublicationPublicationTypeCode2Navigations { get; set; }
        public virtual ICollection<DimOrcidPublication> DimOrcidPublicationPublicationTypeCodeNavigations { get; set; }
        public virtual ICollection<DimOrcidPublication> DimOrcidPublicationTargetAudienceCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationArticleTypeCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationParentPublicationTypeCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationPublicationTypeCode2Navigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationTargetAudienceCodeNavigations { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasets1 { get; set; }
        public virtual ICollection<DimUserChoice> DimUserChoices { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactJufoClassCodesForPubChannel> FactJufoClassCodesForPubChannels { get; set; }

        public virtual ICollection<DimCallProgramme> DimCallProgrammes { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasets { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasetsNavigation { get; set; }
    }
}
