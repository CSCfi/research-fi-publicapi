namespace CSC.PublicApi.Service.Models.ResearchDataset;

/// <summary>
/// Class used to temporary store dataset to dataset relations before refining the data for storing.
/// </summary>
public class DatasetRelationBridge
{
    public string? VersionNumber { get; set; }

    public string? VersionNumber2 { get; set; }

    public int DatabaseId { get; set; }
    
    public string? Id { get; set; }

    public int DatabaseId2 { get; set; }
    
    public string? Id2 { get; set; }

    public string? Type { get; set; }
}