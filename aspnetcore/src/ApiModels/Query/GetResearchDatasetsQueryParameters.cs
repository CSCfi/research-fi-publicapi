using ResearchFi.ResearchDataset;

namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit kun haetaan Rahoitushakuja
/// </summary>
/// <see cref="ResearchDataset"/>
public class GetResearchDatasetsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="ResearchDataset.NameFi"/>
    /// <see cref="ResearchDataset.NameSv"/>
    /// <see cref="ResearchDataset.NameEn"/>
    public string? Name { get; set; }

    /// <summary>
    /// Jokin kentistä descriptionFi, descriptionSv, descriptionEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="ResearchDataset.DescriptionFi"/>
    /// <see cref="ResearchDataset.DescriptionSv"/>
    /// <see cref="ResearchDataset.DescriptionEn"/>
    public string? Description { get; set; }

    /// <summary>
    /// Kentän contributors alakentän organisation alakenttä Id on täsmällleen sama kuin teksti.
    /// </summary>
    /// <see cref="ResearchDataset.Contributors"/>
    /// <see cref="Contributor.Organization"/>
    /// <see cref="Organization.Id"/>
    public string? OrganizationId { get; set; }

    /// <summary>
    /// Jokin kentän contributors alakentän organisation alakentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="ResearchDataset.Contributors"/>
    /// <see cref="Contributor.Organization"/>
    /// <see cref="Organization.NameFi"/>
    /// <see cref="Organization.NameSv"/>
    /// <see cref="Organization.NameEn"/>
    public string? OrganizationName { get; set; }

    /// <summary>
    /// Kentän contributors alakentän person alakenttä name sisältää koko tekstin.
    /// </summary>
    /// <see cref="ResearchDataset.Contributors"/>
    /// <see cref="Contributor.Person"/>
    /// <see cref="Person.Name"/>
    public string? PersonName { get; set; }

    /// <summary>
    /// Jonkin fieldOfScience kentän alakenttä fieldId on täsmälleen sama kuin teksti.
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    /// <see cref="ResearchDataset.FieldsOfScience"/>
    public string? FieldOfScience { get; set; }

    /// <summary>
    /// Jonkin language kentän alakenttä code on täsmälleen sama kuin teksti.
    ///
    /// Koodisto: http://lexvo.org/id/iso639-3
    /// </summary>
    /// <see cref="ResearchDataset.Languages"/>
    public string? Language { get; set; }

    /// <summary>
    /// Kentän accessType alakenttä code on täsmälleen sama kuin teksti.
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/fairdata/access_type
    /// </summary>
    /// <see cref="ResearchDataset.AccessType"/>
    public string? Access { get; set; }

    /// <summary>
    /// Kentän license alakenttä code on täsmälleen sama kuin teksti.
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/fairdata/license
    /// </summary>
    /// <see cref="ResearchDataset.License"/>
    public string? License { get; set; }

    /// <summary>
    /// Jonkin keyword kentän alakenttä value on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="ResearchDataset.Keywords"/>
    /// <see cref="ResearchFi.Keyword.Value"/>
    public string? Keywords { get; set; }

    /// <summary>
    /// Jonkin datasetRelations kentän alakenttä id on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="ResearchDataset.DatasetRelations"/>
    /// <see cref="DatasetRelation.Id"/>
    public string? RelatedDatasetId { get; set; }

    /// <summary>
    /// Kentän researchDataCatalog alakenttä id on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="ResearchDataset.ResearchDataCatalog"/>
    public string? ResearchDataCatalog { get; set; }

    /// <summary>
    /// Jos valinta on tosi palautetaan vain uusimmat version, muuten palautetaan kaikki versiot.
    /// </summary>
    /// <see cref="ResearchDataset.IsLatestVersion"/>
    public bool? IsLatestVersion { get; set; }

    /// <summary>
    /// Luotu aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    /// <see cref="ResearchDataset.Created"/>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Luotu viimeistään. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    /// <see cref="ResearchDataset.Created"/>
    public DateTime? DateTo { get; set; }
}