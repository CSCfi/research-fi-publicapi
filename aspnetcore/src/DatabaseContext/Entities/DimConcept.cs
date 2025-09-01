using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimConcept
{
    public int Id { get; set; }

    public string? Scheme { get; set; }

    public string? ConceptUri { get; set; }

    public string? SchemeUri { get; set; }

    public string? DefinitionFi { get; set; }

    public string? DefinitionEn { get; set; }

    public string? DefinitionSv { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public virtual ICollection<BrConceptRelationship> BrConceptRelationshipDimConceptId2Navigations { get; set; } = new List<BrConceptRelationship>();

    public virtual ICollection<BrConceptRelationship> BrConceptRelationshipDimConcepts { get; set; } = new List<BrConceptRelationship>();

    public virtual ICollection<BrLabelType> BrLabelTypes { get; set; } = new List<BrLabelType>();

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual ICollection<FactConceptContext> FactConceptContexts { get; set; } = new List<FactConceptContext>();
}
