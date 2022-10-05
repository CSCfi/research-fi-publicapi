﻿namespace CSC.PublicApi.DataAccess.Entities;

public partial class DimResearchCommunity
{
    public DimResearchCommunity()
    {
        DimResearcherToResearchCommunities = new HashSet<DimResearcherToResearchCommunity>();
        DimWebLinks = new HashSet<DimWebLink>();
        FactContributions = new HashSet<FactContribution>();
        FactFieldValues = new HashSet<FactFieldValue>();
    }

    public int Id { get; set; }
    public string? Acronym { get; set; }
    public string? NameFi { get; set; }
    public string? NameSv { get; set; }
    public string? NameEn { get; set; }
    public string? NameUnd { get; set; }
    public string? DescriptionFi { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionSv { get; set; }
    public string SourceId { get; set; } = null!;
    public string? SourceDescription { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public int DimRegisteredDataSourceId { get; set; }

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
    public virtual ICollection<DimResearcherToResearchCommunity> DimResearcherToResearchCommunities { get; set; }
    public virtual ICollection<DimWebLink> DimWebLinks { get; set; }
    public virtual ICollection<FactContribution> FactContributions { get; set; }
    public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
}