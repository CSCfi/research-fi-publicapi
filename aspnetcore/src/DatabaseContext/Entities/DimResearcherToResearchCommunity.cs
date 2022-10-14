namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimResearcherToResearchCommunity
{
    public DimResearcherToResearchCommunity()
    {
        FactFieldValues = new HashSet<FactFieldValue>();
    }

    public int Id { get; set; }
    public int DimResearchCommunityId { get; set; }
    public int DimKnownPersonId { get; set; }
    public int StartDate { get; set; }
    public int? EndDate { get; set; }
    public string? DescriptionFi { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionSv { get; set; }
    public string SourceId { get; set; } = null!;
    public string? SourceDescription { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public int DimRegisteredDataSourceId { get; set; }

    public virtual DimKnownPerson DimKnownPerson { get; set; } = null!;
    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
    public virtual DimResearchCommunity DimResearchCommunity { get; set; } = null!;
    public virtual DimDate? EndDateNavigation { get; set; }
    public virtual DimDate StartDateNavigation { get; set; } = null!;
    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
}