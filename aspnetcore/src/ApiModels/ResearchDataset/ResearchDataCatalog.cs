namespace ResearchFi.ResearchDataset;

/// <summary>
/// Data catalog
/// </summary>
public class ResearchDataCatalog
{
    /// <summary>
    /// ID of the data catalog
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the data catalog in Finnish
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Name of the data catalog in Swedish
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Name of the data catalog in English
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Source ID
    /// </summary>
    public string? SourceId { get; set; }

    /// <summary>
    /// Source description
    /// </summary>
    public string? SourceDescription { get; set; }
}