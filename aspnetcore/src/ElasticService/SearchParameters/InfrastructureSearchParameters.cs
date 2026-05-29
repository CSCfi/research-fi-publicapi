namespace CSC.PublicApi.ElasticService.SearchParameters;

public class InfrastructureSearchParameters
{
    public string? KeyIdentifierUrn { get; set; }
    public string? OtherPersistentIdentifier { get; set; }
    public string? LocalIdentifier { get; set; }
    public string? InfraName { get; set; }
    public string? InfraDescription { get; set; }
    public string? Acronym { get; set; }
    public string? Esfri { get; set; }
    public string? FieldOfScience { get; set; }
    public bool? FinlandRoadmap { get; set; }
    public string? ResponsibleOrganizationId { get; set; }
    public string? InfraParticipatingOrganizationsId { get; set; }
    public bool? ExcludeServices { get; set; }
    public int? InfraStartsOnYear { get; set; }
    public int? InfraEndsOnYear { get; set; }
    public string? VisitingAddressCountryCode { get; set; }
}