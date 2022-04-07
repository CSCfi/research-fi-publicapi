using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class BrFundingConsortiumParticipation
    {
        public int DimFundingDecisionId { get; set; }
        public int DimOrganizationid { get; set; }
        public string? RoleInConsortium { get; set; }
        public decimal? ShareOfFundingInEur { get; set; }
        public bool? EndOfParticipation { get; set; }

        public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;
        public virtual DimOrganization DimOrganization { get; set; } = null!;
    }
}
