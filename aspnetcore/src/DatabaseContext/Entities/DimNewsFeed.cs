using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimNewsFeed
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? FeedUrl { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ICollection<DimNewsItem> DimNewsItems { get; set; } = new List<DimNewsItem>();

    public virtual ICollection<FactContribution> FactContributions { get; set; } = new List<FactContribution>();
}
