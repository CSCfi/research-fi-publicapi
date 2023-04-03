using ResearchFi.FundingDecision;

namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit rahoituspäätösten hakemiseen.
/// </summary>
/// <see cref="FundingDecision"/>
public class GetFundingDecisionQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="FundingDecision.NameFi"/>
    /// <see cref="FundingDecision.NameSv"/>
    /// <see cref="FundingDecision.NameEn"/>
    public string? Name { get; set; }

    /// <summary>
    /// Jokin kentistä DescriptionFi, DescriptionSv, DescriptionEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="FundingDecision.DescriptionFi"/>
    /// <see cref="FundingDecision.DescriptionSv"/>
    /// <see cref="FundingDecision.DescriptionEn"/>
    public string? Description { get; set; }

    /// <summary>
    /// Jokin kentän Funder alikentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="FundingDecision.Funder"/>
    /// <see cref="Funder.NameFi"/>
    /// <see cref="Funder.NameSv"/>
    /// <see cref="Funder.NameEn"/>
    public string? FunderName { get; set; }

    /// <summary>
    /// Kentän Funder alikenttä funder:Ids:Content on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="FundingDecision.Funder"/>
    /// <see cref="Funder.Ids"/>
    /// <see cref="PersistentIdentifier.Content"/>
    public string? FunderId { get; set; }

    /// <summary>
    /// Kenttä funderProjectNumber täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="FundingDecision.FunderProjectNumber"/>
    public string? FunderProjectNumber { get; set; }

    /// <summary>
    /// Kenttä fundingStartYear oltava suurempi tai yhtäsuuri kuin teksti.
    /// </summary>
    /// <see cref="FundingDecision.FundingStartYear"/>
    public int? FundingStartYearFrom { get; set; }

    /// <summary>
    /// Jokin kentän OrganizationConsortia alakentistä NameFi, NameSv tai NameEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="FundingDecision.OrganizationConsortia"/>
    /// <see cref="OrganizationConsortium.NameFi"/>
    /// <see cref="OrganizationConsortium.NameSv"/>
    /// <see cref="OrganizationConsortium.NameEn"/>
    public string? FundedOrganization { get; set; }

    /// <summary>
    /// Kentän OrganizationConsortia alakenttä Ids sisältää tekstin.
    /// </summary>
    /// <see cref="FundingDecision.OrganizationConsortia"/>
    /// <see cref="OrganizationConsortium.Ids"/>
    /// <see cref="PersistentIdentifier.Content"/>
    public string? FundedOrganizationId { get; set; }

    /// <summary>
    /// Kenttä fundingGroupPersonFirstName sama kuin teksti.
    /// </summary>
    /// <see cref="FundingDecision.FundingGroupPerson"/>
    /// <see cref="FundingGroupPerson.FirstNames"/>
    public string? FundedPersonFirstName { get; set; }

    /// <summary>
    /// Kenttä fundingGroupPersonLastName sama kuin teksti.
    /// </summary>
    /// <see cref="FundingDecision.FundingGroupPerson"/>
    /// <see cref="FundingGroupPerson.LastName"/>
    public string? FundedPersonLastName { get; set; }

    /// <summary>
    /// Kenttä fundingGroupPersonOrcid sama kuin teksti.
    /// </summary>
    /// <see cref="FundingDecision.FundingGroupPerson"/>
    /// <see cref="FundingGroupPerson.OrcId"/>
    public string? FundedPersonOrcid { get; set; }

    /// <summary>
    /// Kenttä typeOfFunding vastaa haettua tekstiä.
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/rahoitusmuoto
    /// </summary>
    /// <see cref="FundingDecision.TypeOfFunding"/>
    public string? TypeOfFunding { get; set; }
}