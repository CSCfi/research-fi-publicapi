using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class DatasetRelation
{
    [Keyword]
    public string? Id { get; set; }
    
    public string? Type { get; set; }
}