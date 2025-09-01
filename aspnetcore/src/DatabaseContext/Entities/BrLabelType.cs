using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrLabelType
{
    public int DimConceptId { get; set; }

    public int DimKeywordId { get; set; }

    public int DimReferencedataIdLabelType { get; set; }

    public virtual DimConcept DimConcept { get; set; } = null!;

    public virtual DimKeyword DimKeyword { get; set; } = null!;

    public virtual DimReferencedatum DimReferencedataIdLabelTypeNavigation { get; set; } = null!;
}
