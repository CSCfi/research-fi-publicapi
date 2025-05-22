using ResearchFi.CodeList;

namespace ResearchFi.ResearchDataset;

/// <summary>
/// Research data
/// </summary>
public class ResearchDataset
{
    /// <summary>
    /// Local identifier of the dataset
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Name of the research data in Finnish
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Name of the research data in Swedish
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Name of the research data in English
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Description of the research data in Finnish
    /// </summary>
    public string? DescriptionFi { get; set; }

    /// <summary>
    /// Description of the research data in Swedish
    /// </summary>
    public string? DescriptionSv { get; set; }

    /// <summary>
    /// Description of the research data in English
    /// </summary>
    public string? DescriptionEn { get; set; }

    /// <summary>
    /// Creation year
    /// </summary>
    public DateTime? Created { get; set; }

    /// <summary>
    /// Contributors
    /// </summary>
    public List<Contributor>? Contributors { get; set; }

    /// <summary>
    /// Fields of science
    /// 
    /// https://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public List<FieldOfScience>? FieldsOfScience { get; set; }

    /// <summary>
    /// Languages
    /// 
    /// https://lexvo.org/id/iso639-3
    /// </summary>
    public List<LexvoLanguage>? Languages { get; set; }

    /// <summary>
    /// Access types
    /// 
    /// https://uri.suomi.fi/codelist/fairdata/access_type
    /// </summary>
    public AccessType? AccessType { get; set; }

    /// <summary>
    /// License
    /// 
    /// https://uri.suomi.fi/codelist/fairdata/license
    /// </summary>
    public LicenseResearchDataset? License { get; set; }

    /// <summary>
    /// Keywords
    /// </summary>
    public List<Keyword>? Keywords { get; set; }

    /// <summary>
    /// Subject headings
    /// </summary>
    public List<Keyword>? SubjectHeadings { get; set; }

    /// <summary>
    /// Related datasets
    /// </summary>
    public List<DatasetRelation>? DatasetRelations { get; set; }

    /// <summary>
    /// Versions of the dataset
    /// </summary>
    public List<Version>? VersionSet { get; set; }

    /// <summary>
    /// Persistent identifiers
    /// </summary>
    public List<PersistentIdentifier>? PersistentIdentifiers { get; set; }

    /// <summary>
    /// Fairdata URL
    /// </summary>
    public string? FairDataUrl { get; set; }

    /// <summary>
    /// Research data catalog
    /// </summary>
    public ResearchDataCatalog? ResearchDataCatalog { get; set; }

    /// <summary>
    /// Is this dataset the latest version
    /// </summary>
    public bool IsLatestVersion { get; set; }

    /// <summary>
    /// Researchfi portal URL
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }
}