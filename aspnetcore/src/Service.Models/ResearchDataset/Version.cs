using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class Version
{
    [Keyword]
    public string? Identifier { get; set; }
    
    public string? VersionNumber { get; set; }
    
    [Ignore]
    public int DatabaseId { get; set; }
}