using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimName
    {
        public DimName()
        {
            BrParticipatesInFundingGroups = new HashSet<BrParticipatesInFundingGroup>();
            DimFundingDecisions = new HashSet<DimFundingDecision>();
            FactContributions = new HashSet<FactContribution>();
            FactFieldValues = new HashSet<FactFieldValue>();
        }

        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstNames { get; set; }
        /// <summary>
        /// Only to be used, when first name + last name not known (i.e. Metax).
        /// </summary>
        public string? FullName { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int DimKnownPersonIdConfirmedIdentity { get; set; }
        public int DimRegisteredDataSourceId { get; set; }

        public virtual DimKnownPerson DimKnownPersonIdConfirmedIdentityNavigation { get; set; } = null!;
        public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
        public virtual ICollection<BrParticipatesInFundingGroup> BrParticipatesInFundingGroups { get; set; }
        public virtual ICollection<DimFundingDecision> DimFundingDecisions { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
    }
}
