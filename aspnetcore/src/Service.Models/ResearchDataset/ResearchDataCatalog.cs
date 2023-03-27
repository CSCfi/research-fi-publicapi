using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class ResearchDataCatalog
{
    [Keyword]
    public int Id { get; set; }

    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }

    public string? SourceId { get; set; }

    public string? SourceDescription { get; set; }
}