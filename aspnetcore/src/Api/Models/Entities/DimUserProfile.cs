using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimUserProfile
    {
        public DimUserProfile()
        {
            DimFieldDisplaySettings = new HashSet<DimFieldDisplaySetting>();
            DimUserChoices = new HashSet<DimUserChoice>();
            FactFieldValues = new HashSet<FactFieldValue>();
        }

        public int Id { get; set; }
        public int DimKnownPersonId { get; set; }
        public bool AllowAllSubscriptions { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;
        public virtual ICollection<DimFieldDisplaySetting> DimFieldDisplaySettings { get; set; }
        public virtual ICollection<DimUserChoice> DimUserChoices { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
    }
}
