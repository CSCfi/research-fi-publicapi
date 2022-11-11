using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class Version
{
    public string? VersionNumber { get; set; }
    
    [Ignore]
    public int DatabaseId { get; set; }
    
    public string? Identifier { get; set; }
}