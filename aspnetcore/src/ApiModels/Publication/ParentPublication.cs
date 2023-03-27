using ResearchFi.CodeList;

namespace ResearchFi.Publication;

/// <summary>
/// Emojulkaisun tiedot
/// </summary>
public class ParentPublication
{
    /// <summary>
    /// Emojulkaisun nimi
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Emojulkaisun tyyppi
    ///
    /// http://uri.suomi.fi/codelist/research/Emojulkaisuntyyppi
    /// </summary>
    public ParentPublicationType? Type { get; set; }

    /// <summary>
    /// Emojulkaisun toimittajat
    /// </summary>
    public string? Publisher { get; set; }
}