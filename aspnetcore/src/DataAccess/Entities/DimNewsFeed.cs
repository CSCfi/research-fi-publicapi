namespace CSC.PublicApi.DataAccess.Entities;

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
    public string SourceId { get; set; } = null!;
    public string? SourceDescription { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }

    public virtual ICollection<DimNewsItem> DimNewsItems { get; set; }
    public virtual ICollection<FactContribution> FactContributions { get; set; }
}