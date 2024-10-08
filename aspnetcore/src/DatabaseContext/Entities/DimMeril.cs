﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimMeril
{
    public int Id { get; set; }

    public string? NameFi { get; set; }

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ICollection<DimInfrastructure> DimInfrastructures { get; set; } = new List<DimInfrastructure>();
}
