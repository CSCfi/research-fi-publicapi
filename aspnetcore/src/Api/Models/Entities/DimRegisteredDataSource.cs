﻿using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimRegisteredDataSource
    {
        public DimRegisteredDataSource()
        {
            DimAffiliations = new HashSet<DimAffiliation>();
            DimCallProgrammes = new HashSet<DimCallProgramme>();
            DimCompetences = new HashSet<DimCompetence>();
            DimEducations = new HashSet<DimEducation>();
            DimEmailAddrresses = new HashSet<DimEmailAddrress>();
            DimEvents = new HashSet<DimEvent>();
            DimFundingDecisions = new HashSet<DimFundingDecision>();
            DimKeywords = new HashSet<DimKeyword>();
            DimNames = new HashSet<DimName>();
            DimOrcidPublications = new HashSet<DimOrcidPublication>();
            DimOrganizations = new HashSet<DimOrganization>();
            DimPublications = new HashSet<DimPublication>();
            DimResearchActivities = new HashSet<DimResearchActivity>();
            DimResearchCommunities = new HashSet<DimResearchCommunity>();
            DimResearchDatasets = new HashSet<DimResearchDataset>();
            DimResearcherDescriptions = new HashSet<DimResearcherDescription>();
            DimResearcherToResearchCommunities = new HashSet<DimResearcherToResearchCommunity>();
            DimTelephoneNumbers = new HashSet<DimTelephoneNumber>();
            FactFieldValues = new HashSet<FactFieldValue>();
        }

        public int Id { get; set; }
        public int DimOrganizationId { get; set; }
        public string Name { get; set; } = null!;
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Created { get; set; }

        public virtual DimOrganization DimOrganization { get; set; } = null!;
        public virtual ICollection<DimAffiliation> DimAffiliations { get; set; }
        public virtual ICollection<DimCallProgramme> DimCallProgrammes { get; set; }
        public virtual ICollection<DimCompetence> DimCompetences { get; set; }
        public virtual ICollection<DimEducation> DimEducations { get; set; }
        public virtual ICollection<DimEmailAddrress> DimEmailAddrresses { get; set; }
        public virtual ICollection<DimEvent> DimEvents { get; set; }
        public virtual ICollection<DimFundingDecision> DimFundingDecisions { get; set; }
        public virtual ICollection<DimKeyword> DimKeywords { get; set; }
        public virtual ICollection<DimName> DimNames { get; set; }
        public virtual ICollection<DimOrcidPublication> DimOrcidPublications { get; set; }
        public virtual ICollection<DimOrganization> DimOrganizations { get; set; }
        public virtual ICollection<DimPublication> DimPublications { get; set; }
        public virtual ICollection<DimResearchActivity> DimResearchActivities { get; set; }
        public virtual ICollection<DimResearchCommunity> DimResearchCommunities { get; set; }
        public virtual ICollection<DimResearchDataset> DimResearchDatasets { get; set; }
        public virtual ICollection<DimResearcherDescription> DimResearcherDescriptions { get; set; }
        public virtual ICollection<DimResearcherToResearchCommunity> DimResearcherToResearchCommunities { get; set; }
        public virtual ICollection<DimTelephoneNumber> DimTelephoneNumbers { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
    }
}
