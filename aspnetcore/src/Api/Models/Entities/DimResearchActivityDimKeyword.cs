using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimResearchActivityDimKeyword
    {
        public int DimResearchActivityId { get; set; }
        public int DimKeywordId { get; set; }

        public virtual DimResearchActivity DimResearchActivity { get; set; } = null!;
    }
}
