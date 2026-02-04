namespace CSC.PublicApi.ElasticService.SearchParameters;

public class InfrastructureSearchParameters
{
    public string? PersistentIdentifierUrn { get; set; }
    public string? OtherPersistentIdentifier { get; set; }
    public string? LocalIdentifier { get; set; }
    public string? InfraName { get; set; }
    public string? InfraDescription { get; set; }
    public string? Acronym { get; set; }
    public string? Esfri { get; set; }
    public string? FieldOfScience { get; set; }
    public bool? FinlandRoadmapInfrastructure { get; set; }
    public string? ResponsibleOrganizationRor { get; set; }
    public string? ResponsibleOrganizationBusinessId { get; set; }
    public string? OrganizationParticipatesInfrastructureRor { get; set; }
    public string? OrganizationParticipatesInfrastructureBusinessId { get; set; }
    public bool? ExcludeServices { get; set; }
    public int? InfraStartsOnYear { get; set; }
    public int? InfraEndsOnYear { get; set; }
    public string? VisitingAddressCountryCode { get; set; }
}