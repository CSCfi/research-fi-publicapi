﻿using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
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
        public int? DimOrganizationId { get; set; }
        public int? DimKnownPersonId { get; set; }
        public int? DimPublicationId { get; set; }
        public int? DimServiceId { get; set; }
        public int? DimInfrastructureId { get; set; }
        public int? DimPublicationChannelId { get; set; }
        public int? DimResearchDatasetId { get; set; }
        public int? DimFundingDecisionId { get; set; }
        public int? DimResearchDataCatalogId { get; set; }
        public int? DimResearchActivityId { get; set; }
        public int? DimEventId { get; set; }
        public int? DimOrcidPublicationId { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimEvent? DimEvent { get; set; }
        public virtual DimFundingDecision? DimFundingDecision { get; set; }
        public virtual DimInfrastructure? DimInfrastructure { get; set; }
        public virtual DimKnownPerson? DimKnownPerson { get; set; }
        public virtual DimOrcidPublication? DimOrcidPublication { get; set; }
        public virtual DimOrganization? DimOrganization { get; set; }
        public virtual DimPublication? DimPublication { get; set; }
        public virtual DimPublicationChannel? DimPublicationChannel { get; set; }
        public virtual DimResearchActivity? DimResearchActivity { get; set; }
        public virtual DimResearchDataCatalog? DimResearchDataCatalog { get; set; }
        public virtual DimResearchDataset? DimResearchDataset { get; set; }
        public virtual DimService? DimService { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValueDimPidIdOrcidPutCodeNavigations { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValueDimPids { get; set; }
    }
}
