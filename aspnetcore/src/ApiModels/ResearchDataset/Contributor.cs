using ResearchFi.CodeList;

namespace ResearchFi.ResearchDataset;

/// <summary>
/// Tekijä
/// </summary>
public class Contributor
{
    /// <summary>
    /// Organisaatio
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// Tekijä
    /// </summary>
    public Person? Person { get; set; }

    /// <summary>
    /// Rooli
    /// 
    /// http://uri.suomi.fi/codelist/fairdata/agentrole
    /// </summary>
    public AgentRole? Role { get; set; }
}