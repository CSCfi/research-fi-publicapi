using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimOrganisationMedium
    {
        public int DimOrganizationId { get; set; }
        public string? MediaUri { get; set; }
        public string? LanguageVariant { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimOrganization DimOrganization { get; set; } = null!;
    }
}
