using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimSector
    {
        public DimSector()
        {
            DimOrganizations = new HashSet<DimOrganization>();
        }

        public int Id { get; set; }
        public string SectorId { get; set; } = null!;
        public string NameFi { get; set; } = null!;
        public string? NameEn { get; set; }
        public string? NameSv { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ICollection<DimOrganization> DimOrganizations { get; set; }
    }
}
