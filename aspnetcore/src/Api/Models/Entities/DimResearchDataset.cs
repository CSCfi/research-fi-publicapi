﻿using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimResearchDataset
    {
        public DimResearchDataset()
        {
            DimPids = new HashSet<DimPid>();
            DimWebLinks = new HashSet<DimWebLink>();
            FactContributions = new HashSet<FactContribution>();
            FactFieldValues = new HashSet<FactFieldValue>();
            InverseDimResearchDatasetIdPartOfNavigation = new HashSet<DimResearchDataset>();
            InverseDimResearchDatasetIdPreviousVersionNavigation = new HashSet<DimResearchDataset>();
            DimFieldOfSciences = new HashSet<DimFieldOfScience>();
            DimKeywords = new HashSet<DimKeyword>();
            DimReferencedata = new HashSet<DimReferencedatum>();
            DimReferencedataNavigation = new HashSet<DimReferencedatum>();
        }

        public int Id { get; set; }
        public int? DimResearchDataCatalogId { get; set; }
        public int? DimReferencedataAvailability { get; set; }
        public int? DimResearchDatasetIdPartOf { get; set; }
        public int? DimResearchDatasetIdPreviousVersion { get; set; }
        public string? LocalIdentifier { get; set; }
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }
        public string? NameUnd { get; set; }
        public string? DescriptionFi { get; set; }
        public string? DescriptionSv { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionUnd { get; set; }
        public string? VersionInfo { get; set; }
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
        public int DimRegisteredDataSourceId { get; set; }

        public virtual DimReferencedatum? DimReferencedataAvailabilityNavigation { get; set; }
        public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
        public virtual DimResearchDataCatalog? DimResearchDataCatalog { get; set; }
        public virtual DimResearchDataset? DimResearchDatasetIdPartOfNavigation { get; set; }
        public virtual DimResearchDataset? DimResearchDatasetIdPreviousVersionNavigation { get; set; }
        public virtual ICollection<DimPid> DimPids { get; set; }
        public virtual ICollection<DimWebLink> DimWebLinks { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
        public virtual ICollection<DimResearchDataset> InverseDimResearchDatasetIdPartOfNavigation { get; set; }
        public virtual ICollection<DimResearchDataset> InverseDimResearchDatasetIdPreviousVersionNavigation { get; set; }

        public virtual ICollection<DimFieldOfScience> DimFieldOfSciences { get; set; }
        public virtual ICollection<DimKeyword> DimKeywords { get; set; }
        public virtual ICollection<DimReferencedatum> DimReferencedata { get; set; }
        public virtual ICollection<DimReferencedatum> DimReferencedataNavigation { get; set; }
    }
}
