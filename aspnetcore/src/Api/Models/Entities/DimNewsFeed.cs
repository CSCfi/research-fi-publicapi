﻿using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimNewsFeed
    {
        public DimNewsFeed()
        {
            DimNewsItems = new HashSet<DimNewsItem>();
            FactContributions = new HashSet<FactContribution>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? FeedUrl { get; set; }
        public int DimReferencedataId { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual DimReferencedatum DimReferencedata { get; set; } = null!;
        public virtual ICollection<DimNewsItem> DimNewsItems { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
    }
}
