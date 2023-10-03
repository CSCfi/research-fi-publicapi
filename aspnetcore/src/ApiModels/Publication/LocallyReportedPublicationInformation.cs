using ResearchFi.CodeList;

namespace ResearchFi.Publication;

/// <summary>
/// Parallel publication
/// </summary>
public class LocallyReportedPublicationInformation
{
    /// <summary>
    /// URL
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Self archive version
    ///
    /// http://uri.suomi.fi/codelist/research/RinnakkaistallenneVersio
    /// </summary>
    public SelfArchiveVersion? Version { get; set; }
    
    /// <summary>
    /// License
    ///
    /// http://uri.suomi.fi/codelist/research/Lisenssi
    /// </summary>
    public License? License { get; set; }

    /// <summary>
    /// Embargo date
    /// </summary>
    public DateTime? EmbargoDate { get; set; }
}