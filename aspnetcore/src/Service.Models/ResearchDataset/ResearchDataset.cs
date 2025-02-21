using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class ResearchDataset
{
    [Keyword]
    public string? Id { get; set; }

    [Ignore]
    public int DatabaseId { get; set; }

    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }

    public string? DescriptionFi { get; set; }

    public string? DescriptionSv { get; set; }

    public string? DescriptionEn { get; set; }

    public DateTime? Created { get; set; }

    public List<Contributor>? Contributors { get; set; }

    public List<ReferenceData>? FieldsOfScience { get; set; }

    public List<ReferenceData>? Languages { get; set; }

    public ReferenceData? AccessType { get; set; }

    public ReferenceData? License { get; set; }

    public List<Keyword>? Keywords { get; set; }

    public List<DatasetRelation>? DatasetRelations { get; set; }

    public List<PersistentIdentifier>? PersistentIdentifiers { get; set; }
    
    public ResearchDataCatalog? ResearchDataCatalog { get; set; }

    /// <summary>
    /// Intermediate collection for getting outgoing version relations from the database. Not to be stored in Elastic Search.
    /// </summary>
    [Ignore]
    public List<DatasetRelationBridge>? OutgoingDatasetRelations { get; set; }

    /// <summary>
    /// Intermediate collection for getting incoming version relations from the database. Not to be stored in Elastic Search.
    /// </summary>
    [Ignore]
    public List<DatasetRelationBridge>? IncomingDatasetVersionRelations { get; set; }

    /// <summary>
    /// Collection of version references built from incoming and outgoing relations, to be able to include all versions of the data set for all entities.
    /// </summary>
    /// <remarks>
    /// Filled in the in memory operations of the index repository.
    /// </remarks>
    public List<Version>? VersionSet { get; set; }

    /// <summary>
    /// Indicates whether this is the latest version of the dataset or not.
    /// </summary>
    /// <remarks>
    /// Filled in the in memory operations of the index repository.
    /// </remarks>
    public bool? IsLatestVersion { get; set; }

    /// <summary>
    /// Linkki Tutkimustietovarantoon
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }

    public string? LocalIdentifier { get; set; }
}