using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimInfrastructure
{
    public int Id { get; set; }

    public string? Acronym { get; set; }

    public int StartingYear { get; set; }

    public bool FinlandRoadmap { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public virtual ICollection<DimContactInformation> DimContactInformations { get; set; } = new List<DimContactInformation>();

    public virtual ICollection<DimDescriptiveItem> DimDescriptiveItems { get; set; } = new List<DimDescriptiveItem>();

    public virtual ICollection<DimPid> DimPids { get; set; } = new List<DimPid>();

    public virtual ICollection<DimService> DimServices { get; set; } = new List<DimService>();

    public virtual ICollection<DimWebLink> DimWebLinks { get; set; } = new List<DimWebLink>();

    public virtual ICollection<FactContribution> FactContributions { get; set; } = new List<FactContribution>();

    public virtual ICollection<FactDimReferencedataFieldOfScience> FactDimReferencedataFieldOfSciences { get; set; } = new List<FactDimReferencedataFieldOfScience>();

    public virtual ICollection<FactInfraKeyword> FactInfraKeywords { get; set; } = new List<FactInfraKeyword>();

    public virtual ICollection<FactKeyword> FactKeywords { get; set; } = new List<FactKeyword>();

    public virtual ICollection<FactRelation> FactRelationFromInfrastructures { get; set; } = new List<FactRelation>();

    public virtual ICollection<FactRelation> FactRelationToInfrastructures { get; set; } = new List<FactRelation>();

    public virtual ICollection<FactUpkeep> FactUpkeeps { get; set; } = new List<FactUpkeep>();

    public virtual ICollection<DimEsfri> DimEsfris { get; set; } = new List<DimEsfri>();

    public virtual ICollection<DimMeril> DimMerils { get; set; } = new List<DimMeril>();

    public virtual ICollection<DimReferencedatum> DimReferencedata { get; set; } = new List<DimReferencedatum>();
}
