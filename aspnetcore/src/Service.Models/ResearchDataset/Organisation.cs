using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class Organization
{
    [Keyword]
    public string? Id { get; set; }
    
    public List<PersistentIdentifier>? Pids { get; set; }

    public string? NameFi { get; set; }

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public string? NameVariants { get; set; }

}