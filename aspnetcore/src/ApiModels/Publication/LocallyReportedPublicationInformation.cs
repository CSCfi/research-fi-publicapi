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
    /// https://uri.suomi.fi/codelist/research/RinnakkaistallenneVersio
    /// </summary>
    public SelfArchiveVersion? Version { get; set; }
    
    /// <summary>
    /// License
    ///
    /// https://uri.suomi.fi/codelist/research/Lisenssi
    /// </summary>
    public LicensePublication? License { get; set; }

    /// <summary>
    /// Embargo date
    /// </summary>
    public DateTime? EmbargoDate { get; set; }
}