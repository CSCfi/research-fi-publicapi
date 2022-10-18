using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimNewsItem
    {
        public int Id { get; set; }
        public int DimNewsFeedid { get; set; }
        public string? NewsHeadline { get; set; }
        public string? NewsContent { get; set; }
        public string? Url { get; set; }
        public DateTime? Timestamp { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimNewsFeed DimNewsFeed { get; set; } = null!;
    }
}
