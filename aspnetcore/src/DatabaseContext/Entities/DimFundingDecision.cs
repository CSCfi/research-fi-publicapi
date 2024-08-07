﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimFundingDecision
    {
        public DimFundingDecision()
        {
            BrFundingConsortiumParticipations = new HashSet<BrFundingConsortiumParticipation>();
            BrParticipatesInFundingGroups = new HashSet<BrParticipatesInFundingGroup>();
            BrWordClusterDimFundingDecisions = new HashSet<BrWordClusterDimFundingDecision>();
            DimWebLinks = new HashSet<DimWebLink>();
            FactContributions = new HashSet<FactContribution>();
            FactDimReferencedataFieldOfSciences = new HashSet<FactDimReferencedataFieldOfScience>();
            FactFieldValues = new HashSet<FactFieldValue>();
            InverseDimFundingDecisionIdParentDecisionNavigation = new HashSet<DimFundingDecision>();
            DimFundingDecisionFroms = new HashSet<DimFundingDecision>();
            DimFundingDecisionTos = new HashSet<DimFundingDecision>();
            DimKeywords = new HashSet<DimKeyword>();
        }

        public int Id { get; set; }
        public int DimDateIdApproval { get; set; }
        public int DimDateIdStart { get; set; }
        public int DimDateIdEnd { get; set; }
        public int DimNameIdContactPerson { get; set; }
        public int DimCallProgrammeId { get; set; }
        public int DimGeoId { get; set; }
        public int DimTypeOfFundingId { get; set; }
        public int? DimOrganizationIdFunder { get; set; }
        public string? DimPidPidContent { get; set; }
        public int DimFundingDecisionIdParentDecision { get; set; }
        /// <summary>
        /// Päätöksen paikallinen tunniste (tiedon toimittajan)
        /// </summary>
        public string? FunderProjectNumber { get; set; }
        public string? Acronym { get; set; }
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }
        public string? NameUnd { get; set; }
        public string? DescriptionFi { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionSv { get; set; }
        public int? HasInternationalCollaboration { get; set; }
        public int? HasBusinessCollaboration { get; set; }
        public decimal AmountInEur { get; set; }
        public decimal? AmountInFundingDecisionCurrency { get; set; }
        public string? FundingDecisionCurrencyAbbreviation { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int DimRegisteredDataSourceId { get; set; }
        /// <summary>
        /// Rahoituspäätös - Päätöspaneeli
        /// </summary>
        public int? DimCallDecisionsId { get; set; }

        public virtual DimCallDecision? DimCallDecisions { get; set; }
        public virtual DimCallProgramme DimCallProgramme { get; set; } = null!;
        public virtual DimDate DimDateIdApprovalNavigation { get; set; } = null!;
        public virtual DimDate DimDateIdEndNavigation { get; set; } = null!;
        public virtual DimDate DimDateIdStartNavigation { get; set; } = null!;
        public virtual DimFundingDecision DimFundingDecisionIdParentDecisionNavigation { get; set; } = null!;
        public virtual DimGeo DimGeo { get; set; } = null!;
        public virtual DimName DimNameIdContactPersonNavigation { get; set; } = null!;
        public virtual DimOrganization? DimOrganizationIdFunderNavigation { get; set; }
        public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
        public virtual DimReferencedatum DimTypeOfFunding { get; set; } = null!;
        public virtual ICollection<BrFundingConsortiumParticipation> BrFundingConsortiumParticipations { get; set; }
        public virtual ICollection<BrParticipatesInFundingGroup> BrParticipatesInFundingGroups { get; set; }
        public virtual ICollection<BrWordClusterDimFundingDecision> BrWordClusterDimFundingDecisions { get; set; }
        public virtual ICollection<DimWebLink> DimWebLinks { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactDimReferencedataFieldOfScience> FactDimReferencedataFieldOfSciences { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
        public virtual ICollection<DimFundingDecision> InverseDimFundingDecisionIdParentDecisionNavigation { get; set; }

        public virtual ICollection<DimFundingDecision> DimFundingDecisionFroms { get; set; }
        public virtual ICollection<DimFundingDecision> DimFundingDecisionTos { get; set; }
        public virtual ICollection<DimKeyword> DimKeywords { get; set; }
    }
}
