using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class BrParticipatesInFundingGroup
    {
        public int DimFundingDecisionid { get; set; }
        public int DimNameId { get; set; }
        public int DimOrganizationId { get; set; }
        public string? RoleInFundingGroup { get; set; }
        public decimal? ShareOfFundingInEur { get; set; }

        public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;
        public virtual DimName DimName { get; set; } = null!;
        public virtual DimOrganization DimOrganization { get; set; } = null!;
    }
}
