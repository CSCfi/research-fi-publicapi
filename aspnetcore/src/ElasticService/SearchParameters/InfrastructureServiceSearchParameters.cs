namespace CSC.PublicApi.ElasticService.SearchParameters;

public class InfrastructureServiceSearchParameters
{
    public string? PersistentIdentifierUrn { get; set; }
    public string? OtherPersistentIdentifier { get; set; }
    public string? ServiceName { get; set; }
    public string? ServiceDescription { get; set; }
    public bool? ExcludeInfrastructures { get; set; } 
}