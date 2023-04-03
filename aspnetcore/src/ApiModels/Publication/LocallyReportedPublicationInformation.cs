using ResearchFi.CodeList;

namespace ResearchFi.Publication;

/// <summary>
/// Rinnakkaistallenne
/// </summary>
public class LocallyReportedPublicationInformation
{
    /// <summary>
    /// Linkki
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Rinnakkaistallenteen versio
    ///
    /// http://uri.suomi.fi/codelist/research/RinnakkaistallenneVersio
    /// </summary>
    public SelfArchiveVersion? Version { get; set; }
    
    /// <summary>
    /// Lisenssi
    ///
    /// http://uri.suomi.fi/codelist/research/Lisenssi
    /// </summary>
    public License? License { get; set; }

    /// <summary>
    /// Embargo päivämäärä
    /// </summary>
    public DateTime? EmbargoDate { get; set; }
}