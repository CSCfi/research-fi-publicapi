using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimTypeOfFunding
{
    public int Id { get; set; }

    public string TypeId { get; set; } = null!;

    public string NameFi { get; set; } = null!;

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimTypeOfFundingId { get; set; }

    public virtual ICollection<DimProfileOnlyFundingDecision> DimProfileOnlyFundingDecisions { get; set; } = new List<DimProfileOnlyFundingDecision>();

    public virtual DimTypeOfFunding DimTypeOfFundingNavigation { get; set; } = null!;

    public virtual ICollection<DimTypeOfFunding> InverseDimTypeOfFundingNavigation { get; set; } = new List<DimTypeOfFunding>();
}
