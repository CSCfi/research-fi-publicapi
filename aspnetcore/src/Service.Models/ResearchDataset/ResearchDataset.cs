using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class ResearchDataset
{
    private bool? _isLatestVersion;
    private List<Version>? _versions;

    [Ignore]
    public int Id { get; set; }

    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }

    public string? DescriptionFi { get; set; }

    public string? DescriptionSv { get; set; }

    public string? DescriptionEn { get; set; }

    public DateTime? DatasetCreated { get; set; }

    public List<Contributor>? Contributors { get; set; }

    public List<FieldOfScience>? FieldOfSciences { get; set; }

    public List<Language>? Languages { get; set; }

    public Availability? AccessType { get; set; }

    public License? License { get; set; }

    public List<Keyword>? Keywords { get; set; }

    public List<DatasetRelation>? DatasetRelations { get; set; }

    public List<PreferredIdentifier>? PreferredIdentifiers { get; set; }

    public string? LocalIdentifier { get; set; }

    public string? FairDataUrl { get; set; }

    public ResearchDataCatalog? ResearchDataCatalog { get; set; }

    /// <summary>
    /// Intermediate collection for getting outgoing version relations from the database. Not to be stored in Elastic Search.
    /// </summary>
    [Ignore]
    public List<VersionBridge>? OutgoingVersions { get; set; }

    /// <summary>
    /// Intermediate collection for getting incoming version relations from the database. Not to be stored in Elastic Search.
    /// </summary>
    [Ignore]
    public List<VersionBridge>? IncomingVersions { get; set; }

    /// <summary>
    /// Collection of version references built from incoming and outgoing relations, to be able to include all versions of the data set for all entities.
    /// </summary>
    public List<Version> VersionSet
    {
        get
        {
            if (_versions != null)
            {
                return _versions;
            }

            _versions = new List<Version>();

            if (IncomingVersions != null)
            {
                _versions.AddRange(IncomingVersions.Select(version => new Version
                    { Identifier = version.Identifier, VersionNumber = version.VersionNumber }).ToList());
            }

            if (OutgoingVersions != null)
            {
                foreach (var outgoingVersion in OutgoingVersions.Where(outgoingVersion =>
                             _versions.All(s => s.Identifier != outgoingVersion.Identifier2)))
                {
                    _versions.Add(new Version
                    {
                        Identifier = outgoingVersion.Identifier2, VersionNumber = outgoingVersion.VersionNumber2
                    });
                }
            }

            return _versions;
        }
        set => _versions = value;
    }

    public bool IsLatestVersion
    {
        get
        {
            if (_isLatestVersion.HasValue)
            {
                return _isLatestVersion.Value;
            }


            _isLatestVersion = VersionSet.Count == 0 ||
                               VersionSet.Any(s => s.Identifier == Id
                                                   && s.VersionNumber == VersionSet.Max(s => s.VersionNumber));
            return _isLatestVersion.Value;
        }
        set => _isLatestVersion = value;
    }
}