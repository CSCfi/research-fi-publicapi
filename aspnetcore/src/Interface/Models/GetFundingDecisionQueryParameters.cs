using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

public class GetFundingDecisionQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    /// <summary>
    /// Jokin kentistä DescriptionFi, DescriptionSv, DescriptionEn sisältää koko tekstin.
    /// </summary>
    [FromQuery(Name = "description")]
    public string? Description { get; set; }

    /// <summary>
    /// Jokin kentän Funder alikentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    [FromQuery(Name = "funderName")]
    public string? FunderName { get; set; }

    /// <summary>
    /// Kentän Funder alikenttä funderId on täsmälleen sama kuin teksti.
    /// </summary>
    [FromQuery(Name = "funderId")]
    public string? FunderId { get; set; }

    /// <summary>
    /// Kenttä funderProjectNumber täsmälleen sama kuin teksti.
    /// </summary>
    [FromQuery(Name = "funderProjectNumber")]
    public string? FunderProjectNumber { get; set; }

    /// <summary>
    /// Kenttä fundingStartYear oltava suurempi tai yhtäsuuri kuin teksti.
    /// </summary>
    [FromQuery(Name = "fundingStartYearFrom")]
    public int? FundingStartYearFrom { get; set; }

    /// <summary>
    /// Jokin kentän OrganizationConsortia alakentistä NameFi, NameSv tai NameEn sisältää koko tekstin.
    /// </summary>
    [FromQuery(Name = "fundedOrganization")]
    public string? FundedOrganization { get; set; }

    /// <summary>
    /// Kentän OrganizationConsortia alakenttä Ids sisältää tekstin.
    /// </summary>
    [FromQuery(Name = "fundedOrganizationId")]
    public string? FundedOrganizationId { get; set; }

    /// <summary>
    /// Kenttä fundingGroupPersonFirstName sama kuin teksti.
    /// </summary>
    [FromQuery(Name = "fundedPersonFirstName")]
    public string? FundedPersonFirstName { get; set; }

    /// <summary>
    /// Kenttä fundingGroupPersonLastName sama kuin teksti.
    /// </summary>
    [FromQuery(Name = "fundedPersonLastName")]
    public string? FundedPersonLastName { get; set; }

    /// <summary>
    /// Kenttä fundingGroupPersonOrcid sama kuin teksti.
    /// </summary>
    [FromQuery(Name = "fundedPersonOrcid")]
    public string? FundedPersonOrcid { get; set; }

    /// <summary>
    /// Kenttä typeOfFundingID vastaa haettua tekstiä.
    /// </summary>
    [FromQuery(Name = "typeOfFunding")]
    public string? TypeOfFunding { get; set; }
}