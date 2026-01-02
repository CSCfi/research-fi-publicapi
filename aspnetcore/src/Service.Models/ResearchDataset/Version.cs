using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class Version
{
    [Keyword]
    public string? Identifier { get; set; }
    
    public int VersionNumber { get; set; }
}