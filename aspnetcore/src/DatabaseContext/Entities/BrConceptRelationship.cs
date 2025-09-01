using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrConceptRelationship
{
    public int DimReferencedataIdRelationshipTypeCode { get; set; }

    public int DimConceptId { get; set; }

    public int DimConceptId2 { get; set; }

    public virtual DimConcept DimConcept { get; set; } = null!;

    public virtual DimConcept DimConceptId2Navigation { get; set; } = null!;

    public virtual DimReferencedatum DimReferencedataIdRelationshipTypeCodeNavigation { get; set; } = null!;
}
