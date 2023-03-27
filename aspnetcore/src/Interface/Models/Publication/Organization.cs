namespace CSC.PublicApi.Interface.Models.Publication;

/// <summary>
/// Julkaisun Organisaatio
/// </summary>
public class Organization
{
    /// <summary>
    /// Organisaation tunnus
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Organisaation nimi
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// The name of the Organization
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Organisations namn
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Organisaation yksiköt
    /// </summary>
    public List<OrganizationUnit>? Units { get; set; }
}