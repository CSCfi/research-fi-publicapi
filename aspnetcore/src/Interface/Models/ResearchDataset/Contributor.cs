namespace CSC.PublicApi.Interface.Models.ResearchDataset;

public class Contributor
{
    public Organisation? Organisation { get; set; }

    public Person? Person { get; set; }

    public ReferenceData? Role { get; set; }
}