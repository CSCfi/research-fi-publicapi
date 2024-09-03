﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimEducation
{
    public int Id { get; set; }

    public string? LocalIdentifier { get; set; }

    public float? Credits { get; set; }

    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }

    public string? DescriptionFi { get; set; }

    public string? DescriptionSv { get; set; }

    public string? DescriptionEn { get; set; }

    public string? DegreeNameFi { get; set; }

    public string? DegreeNameSv { get; set; }

    public string? DegreeNameEn { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int? DimStartDate { get; set; }

    public int? DimEndDate { get; set; }

    public int? DimInstructionLanguage { get; set; }

    public int DimKnownPersonId { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public string? DegreeGrantingInstitutionName { get; set; }

    public virtual DimDate? DimEndDateNavigation { get; set; }

    public virtual DimReferencedatum? DimInstructionLanguageNavigation { get; set; }

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimDate? DimStartDateNavigation { get; set; }

    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; } = new List<FactFieldValue>();
}
