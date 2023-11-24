using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class FactJufoClassCodesForPubChannel
    {
        public int DimPublicationChannelId { get; set; }
        public int JufoClasses { get; set; }
        public int Year { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimPublicationChannel DimPublicationChannel { get; set; } = null!;
        public virtual DimReferencedatum JufoClassesNavigation { get; set; } = null!;
    }
}
