namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class Contributor
{
    public Organization? Organization { get; set; }
    
    public Person? Person { get; set; }
    
    public ReferenceData? Role { get; set; }

}