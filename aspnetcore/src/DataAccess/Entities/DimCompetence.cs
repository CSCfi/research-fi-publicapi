﻿namespace CSC.PublicApi.DataAccess.Entities;

public partial class DimCompetence
{
    public DimCompetence()
    {
        FactFieldValues = new HashSet<FactFieldValue>();
    }

    public int Id { get; set; }
    public int? CompetenceType { get; set; }
    public int? LocalIdentifier { get; set; }
    public string? NameFi { get; set; }
    public string? NameSv { get; set; }
    public string? NameEn { get; set; }
    public string? DescriptionFi { get; set; }
    public string? DescriptionSv { get; set; }
    public string? DescriptionEn { get; set; }
    public string SourceId { get; set; } = null!;
    public string? SourceDescription { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public int DimKnownPersonId { get; set; }
    public int DimRegisteredDataSourceId { get; set; }

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;
    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
}