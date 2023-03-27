using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class PreferredIdentifier
{
    [Keyword]
    public string? Content { get; set; }
    
    public string? Type { get; set; }
    
}