namespace ResearchFi.Query;

/// <summary>
/// Search parameters for retrieving infrastructure services.
/// </summary>
public class GetInfrastructureServicesQueryParameters
{
    /// <summary>The field persistentIdentifierUrn is exactly equal to the text.</summary>
    public string? PersistentIdentifierUrn { get; set; }

    /// <summary>The field otherPersistentIdentifier is exactly equal to the text.</summary>
    public string? OtherPersistentIdentifier { get; set; }

    /// <summary>The field serviceName contains text.</summary>
    public string? ServiceName { get; set; }

    /// <summary>The field serviceDescription contains text.</summary>
    public string? ServiceDescription { get; set; }

    /// <summary>Exclude related infrastructure (isPartOf) from response data.</summary>
    public bool? ExcludeInfrastructures { get; set; } = false;
}