using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimPid
{
    public int Id { get; set; }

    public string PidContent { get; set; } = null!;

    public string PidType { get; set; } = null!;

    public int DimOrganizationId { get; set; }

    public int DimKnownPersonId { get; set; }

    public int DimPublicationId { get; set; }

    public int DimServiceId { get; set; }

    public int DimInfrastructureId { get; set; }

    public int DimPublicationChannelId { get; set; }

    public int DimResearchDatasetId { get; set; }

    public int DimResearchDataCatalogId { get; set; }

    public int DimResearchActivityId { get; set; }

    public int DimEventId { get; set; }

    public int DimProfileOnlyPublicationId { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int? DimProfileOnlyDatasetId { get; set; }

    public int? DimProfileOnlyFundingDecisionId { get; set; }

    public int? DimResearchProjectId { get; set; }

    public int? DimResearchCommunityId { get; set; }

    public virtual DimEvent DimEvent { get; set; } = null!;

    public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;

    public virtual DimOrganization DimOrganization { get; set; } = null!;

    public virtual DimProfileOnlyDataset? DimProfileOnlyDataset { get; set; }

    public virtual DimProfileOnlyFundingDecision? DimProfileOnlyFundingDecision { get; set; }

    public virtual DimProfileOnlyPublication DimProfileOnlyPublication { get; set; } = null!;

    public virtual DimPublication DimPublication { get; set; } = null!;

    public virtual DimPublicationChannel DimPublicationChannel { get; set; } = null!;

    public virtual DimResearchActivity DimResearchActivity { get; set; } = null!;

    public virtual DimResearchCommunity? DimResearchCommunity { get; set; }

    public virtual DimResearchDataCatalog DimResearchDataCatalog { get; set; } = null!;

    public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;

    public virtual DimService DimService { get; set; } = null!;

    public virtual ICollection<FactFieldValue> FactFieldValueDimPidIdOrcidPutCodeNavigations { get; set; } = new List<FactFieldValue>();

    public virtual ICollection<FactFieldValue> FactFieldValueDimPids { get; set; } = new List<FactFieldValue>();
}
