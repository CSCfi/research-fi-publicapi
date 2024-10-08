﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrWordsDefineACluster
{
    public int DimMinedWordsId { get; set; }

    public int DimWordClusterId { get; set; }

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public string SourceId { get; set; } = null!;

    public virtual DimMinedWord DimMinedWords { get; set; } = null!;

    public virtual DimWordCluster DimWordCluster { get; set; } = null!;
}
