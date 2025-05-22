using ResearchFi.FundingDecision;

namespace ResearchFi.Query;

/// <summary>
/// Query parameters for searching funding decisions.
/// </summary>
/// <see cref="FundingDecision"/>
public class GetFundingDecisionQueryParameters
{
    /// <summary>
    /// One of the fields nameFi, nameSV, nameEn contains the full text.
    /// </summary>
    /// <see cref="FundingDecision.NameFi"/>
    /// <see cref="FundingDecision.NameSv"/>
    /// <see cref="FundingDecision.NameEn"/>
    public string? Name { get; set; }

    /// <summary>
    /// One of the fields DescriptionFi, DescriptionSV, DescriptionEn contains the full text.
    /// </summary>
    /// <see cref="FundingDecision.DescriptionFi"/>
    /// <see cref="FundingDecision.DescriptionSv"/>
    /// <see cref="FundingDecision.DescriptionEn"/>
    public string? Description { get; set; }

    /// <summary>
    /// One of the field Funder subfields nameFi, nameSV, nameEn contains the full text.
    /// </summary>
    /// <see cref="FundingDecision.Funder"/>
    /// <see cref="Funder.NameFi"/>
    /// <see cref="Funder.NameSv"/>
    /// <see cref="Funder.NameEn"/>
    public string? FunderName { get; set; }

    /// <summary>
    /// The Funder field's subfield funder:Ids:Content is exactly equal to the text.
    /// </summary>
    /// <see cref="FundingDecision.Funder"/>
    /// <see cref="Funder.Ids"/>
    /// <see cref="PersistentIdentifier.Content"/>
    public string? FunderId { get; set; }

    /// <summary>
    /// The field funderProjectNumber is exactly equal to the text. 
    /// </summary>
    /// <see cref="FundingDecision.FunderProjectNumber"/>
    public string? FunderProjectNumber { get; set; }

    /// <summary>
    /// The field fundingStartYear must be equal or greater than the text.
    /// </summary>
    /// <see cref="FundingDecision.FundingStartDate"/>
    public int? FundingStartYearFrom { get; set; }

    /// <summary>
    /// One of the fields in the OrganizationConsortia subfields NameFi, NameSV or NameEn contains the full text.
    /// </summary>
    /// <see cref="FundingDecision.OrganizationConsortia"/>
    /// <see cref="OrganizationConsortium.NameFi"/>
    /// <see cref="OrganizationConsortium.NameSv"/>
    /// <see cref="OrganizationConsortium.NameEn"/>
    public string? FundedOrganization { get; set; }

    /// <summary>
    /// The field OrganizationConsortia subfield Ids contains text.
    /// </summary>
    /// <see cref="FundingDecision.OrganizationConsortia"/>
    /// <see cref="OrganizationConsortium.Ids"/>
    /// <see cref="PersistentIdentifier.Content"/>
    public string? FundedOrganizationId { get; set; }

    /// <summary>
    /// The field fundingGroupPersonFirstName is exactly equal to the text.
    /// </summary>
    /// <see cref="FundingDecision.FundingGroupPerson"/>
    /// <see cref="FundingGroupPerson.FirstNames"/>
    public string? FundedPersonFirstName { get; set; }

    /// <summary>
    /// The field fundingGroupPersonLastName is exactly equal to the text.
    /// </summary>
    /// <see cref="FundingDecision.FundingGroupPerson"/>
    /// <see cref="FundingGroupPerson.LastName"/>
    public string? FundedPersonLastName { get; set; }

    /// <summary>
    /// The field fundingGroupPersonOrcid is exactly equal to the text.
    /// </summary>
    /// <see cref="FundingDecision.FundingGroupPerson"/>
    /// <see cref="FundingGroupPerson.OrcId"/>
    public string? FundedPersonOrcid { get; set; }

    /// <summary>
    /// The field typeOfFunding is exactly equal to the text.
    ///
    /// Code: https://uri.suomi.fi/codelist/research/rahoitusmuoto
    /// </summary>
    /// <see cref="FundingDecision.TypeOfFunding"/>
    public string? TypeOfFunding { get; set; }
}