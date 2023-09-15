using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimReferencedatum
    {
        public DimReferencedatum()
        {
            BrGrantedPermissions = new HashSet<BrGrantedPermission>();
            DimAffiliations = new HashSet<DimAffiliation>();
            DimEducations = new HashSet<DimEducation>();
            DimLocallyReportedPubInfoSelfArchivedLicenseCodeNavigations = new HashSet<DimLocallyReportedPubInfo>();
            DimLocallyReportedPubInfoSelfArchivedVersionCodeNavigations = new HashSet<DimLocallyReportedPubInfo>();
            DimProfileOnlyDatasets = new HashSet<DimProfileOnlyDataset>();
            DimProfileOnlyPublicationArticleTypeCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationLanguageCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationLicenseCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationParentTypeClassificationCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationPublicationCountryCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationPublicationFormatCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationTargetAudienceCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationThesisTypeCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimProfileOnlyPublicationTypeClassificationCodeNavigations = new HashSet<DimProfileOnlyPublication>();
            DimPublicationArticleTypeCodeNavigations = new HashSet<DimPublication>();
            DimPublicationLanguageCodeNavigations = new HashSet<DimPublication>();
            DimPublicationLicenseCodeNavigations = new HashSet<DimPublication>();
            DimPublicationParentPublicationTypeCodeNavigations = new HashSet<DimPublication>();
            DimPublicationPublicationCountryCodeNavigations = new HashSet<DimPublication>();
            DimPublicationPublicationTypeCode2Navigations = new HashSet<DimPublication>();
            DimPublicationPublicationTypeCodeNavigations = new HashSet<DimPublication>();
            DimPublicationTargetAudienceCodeNavigations = new HashSet<DimPublication>();
            DimPublicationThesisTypeCodeNavigations = new HashSet<DimPublication>();
            DimResearchDatasetDimReferencedataAvailabilityNavigations = new HashSet<DimResearchDataset>();
            DimResearchDatasetDimReferencedataLicenseNavigations = new HashSet<DimResearchDataset>();
            DimUserChoices = new HashSet<DimUserChoice>();
            FactContributions = new HashSet<FactContribution>();
            FactDimReferencedataFieldOfSciences = new HashSet<FactDimReferencedataFieldOfScience>();
            FactFieldValueDimReferencedataActorRoles = new HashSet<FactFieldValue>();
            FactFieldValueDimReferencedataFieldOfSciences = new HashSet<FactFieldValue>();
            FactJufoClassCodesForPubChannels = new HashSet<FactJufoClassCodesForPubChannel>();
            InverseDimReferencedata = new HashSet<DimReferencedatum>();
            DimCallProgrammes = new HashSet<DimCallProgramme>();
            DimPublications = new HashSet<DimPublication>();
            DimResearchDatasets = new HashSet<DimResearchDataset>();
        }

        public int Id { get; set; }
        public string CodeScheme { get; set; } = null!;
        public string CodeValue { get; set; } = null!;
        public string? NameFi { get; set; }
        public string? NameEn { get; set; }
        public string? NameSv { get; set; }
        public string SourceId { get; set; } = null!;
        public string SourceDescription { get; set; } = null!;
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string? State { get; set; }
        public int DimReferencedataId { get; set; }
        public int? Order { get; set; }

        public virtual DimReferencedatum DimReferencedata { get; set; } = null!;
        public virtual ICollection<BrGrantedPermission> BrGrantedPermissions { get; set; }
        public virtual ICollection<DimAffiliation> DimAffiliations { get; set; }
        public virtual ICollection<DimEducation> DimEducations { get; set; }
        public virtual ICollection<DimLocallyReportedPubInfo> DimLocallyReportedPubInfoSelfArchivedLicenseCodeNavigations { get; set; }
        public virtual ICollection<DimLocallyReportedPubInfo> DimLocallyReportedPubInfoSelfArchivedVersionCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyDataset> DimProfileOnlyDatasets { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationArticleTypeCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationLanguageCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationLicenseCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationParentTypeClassificationCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationPublicationCountryCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationPublicationFormatCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationTargetAudienceCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationThesisTypeCodeNavigations { get; set; }
        public virtual ICollection<DimProfileOnlyPublication> DimProfileOnlyPublicationTypeClassificationCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationArticleTypeCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationLanguageCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationLicenseCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationParentPublicationTypeCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationPublicationCountryCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationPublicationTypeCode2Navigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationPublicationTypeCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationTargetAudienceCodeNavigations { get; set; }
        public virtual ICollection<DimPublication> DimPublicationThesisTypeCodeNavigations { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasetDimReferencedataAvailabilityNavigations { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasetDimReferencedataLicenseNavigations { get; set; }
        public virtual ICollection<DimUserChoice> DimUserChoices { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactDimReferencedataFieldOfScience> FactDimReferencedataFieldOfSciences { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValueDimReferencedataActorRoles { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValueDimReferencedataFieldOfSciences { get; set; }
        public virtual ICollection<FactJufoClassCodesForPubChannel> FactJufoClassCodesForPubChannels { get; set; }
        public virtual ICollection<DimReferencedatum> InverseDimReferencedata { get; set; }

        public virtual ICollection<DimCallProgramme> DimCallProgrammes { get; set; }
        public virtual ICollection<DimPublication> DimPublications { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasets { get; set; }
    }
}
