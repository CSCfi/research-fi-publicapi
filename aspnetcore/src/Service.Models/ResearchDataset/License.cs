using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class License
{
    [Keyword]
    public string? Code { get; set; }

    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }
}