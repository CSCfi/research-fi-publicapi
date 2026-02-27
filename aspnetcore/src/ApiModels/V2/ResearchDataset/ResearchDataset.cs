using ResearchFi.CodeList;
using ResearchFi.ResearchDataset.Common;
using Version = ResearchFi.ResearchDataset.Common.Version;

namespace ResearchFi.ResearchDataset.V2;

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
    /// Is this dataset the latest version
    /// </summary>
    public bool IsLatestVersion { get; set; }

    /// <summary>
    /// Researchfi portal URL
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }
}