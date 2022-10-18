using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class FactUpkeep
    {
        public int DimOrganizationId { get; set; }
        public int DimGeoId { get; set; }
        public int DimInfrastructureId { get; set; }
        public int DimServiceId { get; set; }
        public int DimServicePointId { get; set; }
        public int DimDateIdStart { get; set; }
        public int DimDateIdEnd { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimDate DimDateIdEndNavigation { get; set; } = null!;
        public virtual DimDate DimDateIdStartNavigation { get; set; } = null!;
        public virtual DimGeo DimGeo { get; set; } = null!;
        public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;
        public virtual DimOrganization DimOrganization { get; set; } = null!;
        public virtual DimService DimService { get; set; } = null!;
        public virtual DimServicePoint DimServicePoint { get; set; } = null!;
    }
}
