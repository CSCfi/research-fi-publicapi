using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactDimReferencedataFieldOfScience
{
    public int DimReferencedataId { get; set; }

    public int DimResearchDatasetId { get; set; }

    public int DimKnownPersonId { get; set; }

    public int DimPublicationId { get; set; }

    public int DimResearchActivityId { get; set; }

    public int DimFundingDecisionId { get; set; }

    public int DimInfrastructureId { get; set; }

    public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;

    public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;

    public virtual DimPublication DimPublication { get; set; } = null!;

    public virtual DimReferencedatum DimReferencedata { get; set; } = null!;

    public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;
}
