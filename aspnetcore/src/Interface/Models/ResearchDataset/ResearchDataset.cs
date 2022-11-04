namespace CSC.PublicApi.Interface.Models.ResearchDataset;

public class ResearchDataset
{
    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }

    public string? DescriptionFi { get; set; }

    public string? DescriptionSv { get; set; }

    public string? DescriptionEn { get; set; }

    public DateTime? DatasetCreated { get; set; }

    public List<Contributor> Contributors { get; set; }

    public List<FieldOfScience>? FieldOfSciences { get; set; }

    public List<Language>? Languages { get; set; }

    public Availability? AccessType { get; set; }

    public License? License { get; set; }

    public List<Keyword>? Keywords { get; set; }

    public List<DatasetRelation> DatasetRelations { get; set; }

    public List<Version> VersionSet { get; set; }

    public List<PreferredIdentifier>? PreferredIdentifiers { get; set; }

    public string? Identifier { get; set; }

    public string? FairDataUrl { get; set; }

    public ResearchDataCatalog ResearchDataCatalog { get; set; }

    public bool IsLatestVersion { get; set; }

}