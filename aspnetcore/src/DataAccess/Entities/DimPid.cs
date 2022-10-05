namespace CSC.PublicApi.DataAccess.Entities;

public partial class DimPid
{
    public DimPid()
    {
        FactFieldValueDimPidIdOrcidPutCodeNavigations = new HashSet<FactFieldValue>();
        FactFieldValueDimPids = new HashSet<FactFieldValue>();
    }

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
    public int DimFundingDecisionId { get; set; }
    public int DimResearchDataCatalogId { get; set; }
    public int DimResearchActivityId { get; set; }
    public int DimEventId { get; set; }
    public int DimOrcidPublicationId { get; set; }
    public string SourceId { get; set; } = null!;
    public string? SourceDescription { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }

    public virtual DimEvent DimEvent { get; set; } = null!;
    public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;
    public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;
    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;
    public virtual DimOrcidPublication DimOrcidPublication { get; set; } = null!;
    public virtual DimOrganization DimOrganization { get; set; } = null!;
    public virtual DimPublication DimPublication { get; set; } = null!;
    public virtual DimPublicationChannel DimPublicationChannel { get; set; } = null!;
    public virtual DimResearchActivity DimResearchActivity { get; set; } = null!;
    public virtual DimResearchDataCatalog DimResearchDataCatalog { get; set; } = null!;
    public virtual DimService DimService { get; set; } = null!;
    public virtual ICollection<FactFieldValue> FactFieldValueDimPidIdOrcidPutCodeNavigations { get; set; }
    public virtual ICollection<FactFieldValue> FactFieldValueDimPids { get; set; }
}