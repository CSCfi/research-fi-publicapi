using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class Contributor
{
    public Organisation? Organisation { get; set; }
    
    public Person? Person { get; set; }
    
    public ReferenceData? Role { get; set; }

}