﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimResearchDataset
    {
        public DimResearchDataset()
        {
            BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations = new HashSet<BrDatasetDatasetRelationship>();
            BrDatasetDatasetRelationshipDimResearchDatasets = new HashSet<BrDatasetDatasetRelationship>();
            DimPids = new HashSet<DimPid>();
            DimWebLinks = new HashSet<DimWebLink>();
            FactContributions = new HashSet<FactContribution>();
            FactDimReferencedataFieldOfSciences = new HashSet<FactDimReferencedataFieldOfScience>();
            FactFieldValues = new HashSet<FactFieldValue>();
            DimKeywords = new HashSet<DimKeyword>();
            DimReferencedata = new HashSet<DimReferencedatum>();
        }

        public int Id { get; set; }
        public int? DimResearchDataCatalogId { get; set; }
        public int? DimReferencedataLicense { get; set; }
        public int? DimReferencedataAvailability { get; set; }
        public string? LocalIdentifier { get; set; }
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionFi { get; set; }
        public string? DescriptionSv { get; set; }
        public string? DescriptionEn { get; set; }
        public bool? InternationalCollaboration { get; set; }
        public DateTime? DatasetCreated { get; set; }
        public DateTime? DatasetModified { get; set; }
        public DateTime? TemporalCoverageStart { get; set; }
        public DateTime? TemporalCoverageEnd { get; set; }
        public string? GeographicCoverage { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string? NameUnd { get; set; }
        public string? DescriptionUnd { get; set; }
        public int DimRegisteredDataSourceId { get; set; }
        public string? VersionInfo { get; set; }

        public virtual DimReferencedatum? DimReferencedataAvailabilityNavigation { get; set; }
        public virtual DimReferencedatum? DimReferencedataLicenseNavigation { get; set; }
        public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
        public virtual DimResearchDataCatalog? DimResearchDataCatalog { get; set; }
        public virtual ICollection<BrDatasetDatasetRelationship> BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations { get; set; }
        public virtual ICollection<BrDatasetDatasetRelationship> BrDatasetDatasetRelationshipDimResearchDatasets { get; set; }
        public virtual ICollection<DimPid> DimPids { get; set; }
        public virtual ICollection<DimWebLink> DimWebLinks { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactDimReferencedataFieldOfScience> FactDimReferencedataFieldOfSciences { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }

        public virtual ICollection<DimKeyword> DimKeywords { get; set; }
        public virtual ICollection<DimReferencedatum> DimReferencedata { get; set; }
    }
}
