namespace CSC.PublicApi.ElasticService.SearchParameters;

public class InfrastructureServiceSearchParameters
{
    public string? PersistentIdentifierUrn { get; set; }
    public string? OtherPersistentIdentifier { get; set; }
    public string? LocalIdentifier { get; set; }
    public string? ServiceName { get; set; }
    public string? ServiceDescription { get; set; }
    public string? IsPartOfInfrastructureURN { get; set; }
    public string? IsPartOfInfrastructureResponsibleOrganization { get; set; }
    public int? ServiceStartsOnYear { get; set; }
    public int? ServiceEndsOnYear { get; set; }
    public int? ServiceEndsByYear { get; set; }
}