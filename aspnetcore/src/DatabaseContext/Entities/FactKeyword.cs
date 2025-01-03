using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactKeyword
{
    public int DimKeywordId { get; set; }

    public int DimResearchProjectId { get; set; }

    public string SourceId { get; set; } = null!;

    public string SourceDescription { get; set; } = null!;

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }
}
