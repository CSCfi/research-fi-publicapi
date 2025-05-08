using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

/// <summary>
/// Rahoituspäätöspaneeli
/// </summary>
public partial class DimCallDecision
{
    public int Id { get; set; }

    public int DecisionMaker { get; set; }

    public int DimDateIdApproval { get; set; }

    public int DimCallProgrammeId { get; set; }

    /// <summary>
    /// Rahoituspäätöspaneeli - Haun vaihe
    /// </summary>
    public string CallProcessingPhase { get; set; } = null!;

    public string? SourceId { get; set; }

    public string? SourceDescription { get; set; }

    public virtual DimReferencedatum DecisionMakerNavigation { get; set; } = null!;

    public virtual DimCallProgramme DimCallProgramme { get; set; } = null!;

    public virtual DimDate DimDateIdApprovalNavigation { get; set; } = null!;

    public virtual ICollection<DimFundingDecision> DimFundingDecisions { get; set; } = new List<DimFundingDecision>();
}
