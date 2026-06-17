using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimInternationalInfra
{
    public int Id { get; set; }

    public string NameEn { get; set; } = null!;

    public string? UnlinkedIdentifier { get; set; }

    public string? Weblink { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ICollection<FactRelation> FactRelationFromInternationalInfras { get; set; } = new List<FactRelation>();

    public virtual ICollection<FactRelation> FactRelationToInternationalInfras { get; set; } = new List<FactRelation>();
}
