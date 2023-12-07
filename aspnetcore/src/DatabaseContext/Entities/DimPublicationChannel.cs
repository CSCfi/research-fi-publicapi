using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimPublicationChannel
    {
        public DimPublicationChannel()
        {
            DimPids = new HashSet<DimPid>();
            DimPublications = new HashSet<DimPublication>();
            DimResearchActivities = new HashSet<DimResearchActivity>();
        }

        public int Id { get; set; }
        public string? JufoCode { get; set; }
        public string? ChannelNameAnylang { get; set; }
        public string? PublisherNameText { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ICollection<DimPid> DimPids { get; set; }
        public virtual ICollection<DimPublication> DimPublications { get; set; }
        public virtual ICollection<DimResearchActivity> DimResearchActivities { get; set; }
    }
}
