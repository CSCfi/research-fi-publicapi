using ResearchFi.ResearchDataset;

namespace ResearchFi.Query;

/// <summary>
/// Query parameters for searching research sets.
/// </summary>
/// <see cref="ResearchDataset"/>
public class GetResearchDatasetsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// One of the fields nameFi, nameSV, nameEn contains the full text.
    /// </summary>
    /// <see cref="ResearchDataset.NameFi"/>
    /// <see cref="ResearchDataset.NameSv"/>
    /// <see cref="ResearchDataset.NameEn"/>
    public string? Name { get; set; }

    /// <summary>
    /// One of the fields descriptionFi, descriptionSV, descriptionEn contains the full text.
    /// </summary>
    /// <see cref="ResearchDataset.DescriptionFi"/>
    /// <see cref="ResearchDataset.DescriptionSv"/>
    /// <see cref="ResearchDataset.DescriptionEn"/>
    public string? Description { get; set; }

    /// <summary>
    /// The field contributors subfield organization subfield Id is exactly the same as the text.
    /// </summary>
    /// <see cref="ResearchDataset.Contributors"/>
    /// <see cref="Contributor.Organization"/>
    /// <see cref="Organization.Id"/>
    public string? OrganizationId { get; set; }

    /// <summary>
    /// One of the fields in contributors subfield organization NameFi, NameSV, NameEn contains the full text.
    /// </summary>
    /// <see cref="ResearchDataset.Contributors"/>
    /// <see cref="Contributor.Organization"/>
    /// <see cref="Organization.NameFi"/>
    /// <see cref="Organization.NameSv"/>
    /// <see cref="Organization.NameEn"/>
    public string? OrganizationName { get; set; }

    /// <summary>
    /// The field contributors subfield person subfield name contains the full text.
    /// </summary>
    /// <see cref="ResearchDataset.Contributors"/>
    /// <see cref="Contributor.Person"/>
    /// <see cref="Person.Name"/>
    public string? PersonName { get; set; }

    /// <summary>
    /// One subfield fieldId of field FieldOfScience is exactly equal to the text.
    ///
    /// Code: https://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    /// <see cref="ResearchDataset.FieldsOfScience"/>
    public string? FieldOfScience { get; set; }

    /// <summary>
    /// One subfield code of field language is exactly equal to the text.
    ///
    /// Code: https://lexvo.org/id/iso639-3
    /// </summary>
    /// <see cref="ResearchDataset.Languages"/>
    public string? Language { get; set; }

    /// <summary>
    /// The field AccessType subfield code is exactly equal to the text.
    ///
    /// Code: https://uri.suomi.fi/codelist/fairdata/access_type
    /// </summary>
    /// <see cref="ResearchDataset.AccessType"/>
    public string? Access { get; set; }

    /// <summary>
    /// The field license subfield code is exactly the same as the text.
    ///
    /// Code: https://uri.suomi.fi/codelist/fairdata/license
    /// </summary>
    /// <see cref="ResearchDataset.License"/>
    public string? License { get; set; }

    /// <summary>
    /// One subfield value of field keyword is exactly equal to the text.
    /// </summary>
    /// <see cref="ResearchDataset.Keywords"/>
    /// <see cref="ResearchFi.Keyword.Value"/>
    public string? Keywords { get; set; }

    /// <summary>
    /// One subfield value of field subject heading is exactly equal to the text.
    /// </summary>
    /// <see cref="ResearchDataset.SubjectHeadings"/>
    /// <see cref="ResearchFi.Keyword.Value"/>
    public string? SubjectHeadings { get; set; }

    /// <summary>
    /// One subfield id of field datasetRelations is exactly equal to the text.
    /// </summary>
    /// <see cref="ResearchDataset.DatasetRelations"/>
    /// <see cref="DatasetRelation.Id"/>
    public string? RelatedDatasetId { get; set; }

    /// <summary>
    /// The field researchDataCatalog subfield id is exactly equal to the text.
    /// </summary>
    /// <see cref="ResearchDataset.ResearchDataCatalog"/>
    public string? ResearchDataCatalog { get; set; }

    /// <summary>
    /// If the selection is true, only the latest version will be returned, otherwise all versions will be returned.
    /// </summary>
    /// <see cref="ResearchDataset.IsLatestVersion"/>
    public bool? IsLatestVersion { get; set; }

    /// <summary>
    /// Created at the earliest. Date format yyyy-mm-dd
    /// </summary>
    /// <see cref="ResearchDataset.Created"/>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Created at the latest. Date format yyyy-mm-dd
    /// </summary>
    /// <see cref="ResearchDataset.Created"/>
    public DateTime? DateTo { get; set; }
}