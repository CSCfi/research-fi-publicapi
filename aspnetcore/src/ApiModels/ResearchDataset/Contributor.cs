using ResearchFi.CodeList;

namespace ResearchFi.ResearchDataset;

/// <summary>
/// Contributor
/// </summary>
public class Contributor
{
    /// <summary>
    /// Contributing organization
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// Contributing person
    /// </summary>
    public Person? Person { get; set; }

    /// <summary>
    /// Agent role
    /// 
    /// https://uri.suomi.fi/codelist/fairdata/agentrole
    /// </summary>
    public AgentRole? Role { get; set; }
}