using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimResearcherDescription
{
    public int Id { get; set; }

    public int? DescriptionType { get; set; }

    public string? ResearchDescriptionFi { get; set; }

    public string? ResearchDescriptionEn { get; set; }

    public string? ResearchDescriptionSv { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimKnownPersonId { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; } = new List<FactFieldValue>();
}
