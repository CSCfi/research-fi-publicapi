namespace ResearchFi.Query;

/// <summary>
/// Search parameters for retrieving infrastructures.
/// </summary>
public class GetInfrastructuresQueryParameters
{
    /// <summary>The field infraIdentifier.keyIdentifierURN is exactly equal to the text.</summary>
    public string? KeyIdentifierUrn { get; set; }

    /// <summary>The field infraIdentifier.otherPersistentIdentifier is exactly equal to the text.</summary>
    public string? OtherPersistentIdentifier { get; set; }

    /// <summary>The field infraIdentifier.localIdentifier is exactly equal to the text.</summary>
    public string? LocalIdentifier { get; set; }

    /// <summary>The field infraName contains text.</summary>
    public string? InfraName { get; set; }

    /// <summary>The field infraDescription contains text.</summary>
    public string? InfraDescription { get; set; }

    /// <summary>The field acronym is exactly equal to the text.</summary>
    public string? Acronym { get; set; }

    /// <summary>
    /// One subfield codeValue of field esfri is exactly equal to the text.
    /// 
    /// Code:https://uri.suomi.fi/codelist/research/ESFRI-Domain
    /// </summary>
    public string? Esfri { get; set; }

    /// <summary>
    /// One subfield codeValue of field fieldOfScience is exactly equal to the text.
    /// 
    /// Code: https://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public string? FieldOfScience { get; set; }

    /// <summary>
    /// If selection is true, return only those infrastucrues, which are on the roadmap for Finnish Research Infrastructures.
    /// 
    /// If selection is false, return only those infrastructures, which are not on the roadmap for Finnish Research Infrastructures.
    /// </summary>
    public bool? FinlandRoadmap { get; set; }

    /// <summary>List infraResponsibleOrganization.organizationIdentifier contains field pid exactly equal to the text.</summary>
    public string? InfraResponsibleOrganizationId { get; set; }

    /// <summary>List infraParticipatingOrganizations.organizationIdentifier contains field pid exactly equal to the text.</summary>
    public string? InfraParticipatingOrganizationsId { get; set; }

    /// <summary>The field infraStartsOn.year is equal.</summary>
    public int? InfraStartsOnYear { get; set; }

    /// <summary>The field infraEndsOn.year is equal.</summary>
    public int? InfraEndsOnYear { get; set; }

    /// <summary>
    /// One field visitingAddress.country.codeValue in list infraContactInformation is exactly equal to the text
    /// 
    /// Code: https://koodistot.suomi.fi/codescheme;registryCode=jhs;schemeCode=valtio_1_20120101
    /// </summary>
    public string? VisitingAddressCountryCode { get; set; }

    /// <summary>Exclude related services (InfraServices) from response data.</summary>
    public bool? ExcludeServices { get; set; } = false;
}