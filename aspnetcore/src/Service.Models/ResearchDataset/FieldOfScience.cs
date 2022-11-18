using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class FieldOfScience
{
    [Keyword]
    public string? FieldId { get; set; }
    public string? NameFi { get; set; }
    public string? NameSv { get; set; }
    public string? NameEn { get; set; }
}