namespace ResearchFi.Query;

/// <summary>
/// Search parameters for retrieving infrastructure services.
/// </summary>
public class GetInfrastructureServicesQueryParameters
{
    /// <summary>The field serviceIdentifier.persistentIdentifierURN is exactly equal to the text.</summary>
    public string? PersistentIdentifierUrn { get; set; }

    /// <summary>The field serviceIdentifier.otherPersistentIdentifier is exactly equal to the text.</summary>
    public string? OtherPersistentIdentifier { get; set; }

    /// <summary>The field serviceIdentifier.localIdentifier is exactly equal to the text.</summary>
    public string? LocalIdentifier { get; set; }

    /// <summary>The field serviceName contains text.</summary>
    public string? ServiceName { get; set; }

    /// <summary>The field serviceDescription contains text.</summary>
    public string? ServiceDescription { get; set; }

    /// <summary>The field isPartOfInfrastructure.infraIdentifier.persistentIdentifierURN is exactly equal to the text.</summary>
    public string? IsPartOfInfrastructureURN { get; set; }

    /// <summary>
    /// Responsible organization of the infrastructure offering the service.
    /// 
    /// List isPartOfInfrastructure.responsibleOrganization.organizationIdentifier contains field pid exactly equal to the text.
    /// </summary>
    public string? IsPartOfInfrastructureResponsibleOrganizationId { get; set; }

    /// <summary>
    /// Participating organization of the infrastructure offering the service.
    /// 
    /// List isPartOfInfrastructure.organizationParticipatesInfrastructure.organizationIdentifier contains field pid exactly equal to the text.
    /// </summary>
    public string? IsPartOfInfrastructureOrganizationParticipatesInfrastructureId { get; set; }

    /// <summary>The field serviceStartsOn.year is exactly equal</summary>
    public int? ServiceStartsOnYear { get; set; }
    
    /// <summary>The field serviceEndsOn.year is exactly equal.</summary>
    public int? ServiceEndsOnYear { get; set; }
    
    /// <summary>The field serviceEndsOn.year is less than or equal.</summary>
    public int? ServiceEndsByYear { get; set; }
}