using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class FactContribution
    {
        public int DimFundingDecisionId { get; set; }
        public int DimOrganizationId { get; set; }
        public int DimDateId { get; set; }
        public int DimNameId { get; set; }
        public int DimPublicationId { get; set; }
        public int DimGeoId { get; set; }
        public int DimInfrastructureId { get; set; }
        public int DimNewsFeedId { get; set; }
        public int DimResearchDatasetId { get; set; }
        public int DimResearchDataCatalogId { get; set; }
        public int DimIdentifierlessDataId { get; set; }
        public int DimResearchActivityId { get; set; }
        public int DimResearchCommunityId { get; set; }
        public int DimReferencedataActorRoleId { get; set; }
        public string ContributionType { get; set; } = null!;
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimDate DimDate { get; set; } = null!;
        public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;
        public virtual DimGeo DimGeo { get; set; } = null!;
        public virtual DimIdentifierlessDatum DimIdentifierlessData { get; set; } = null!;
        public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;
        public virtual DimName DimName { get; set; } = null!;
        public virtual DimNewsFeed DimNewsFeed { get; set; } = null!;
        public virtual DimOrganization DimOrganization { get; set; } = null!;
        public virtual DimPublication DimPublication { get; set; } = null!;
        public virtual DimReferencedatum DimReferencedataActorRole { get; set; } = null!;
        public virtual DimResearchActivity DimResearchActivity { get; set; } = null!;
        public virtual DimResearchCommunity DimResearchCommunity { get; set; } = null!;
        public virtual DimResearchDataCatalog DimResearchDataCatalog { get; set; } = null!;
        public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;
    }
}
