using ResearchFi.CodeList;

namespace ResearchFi.Publication;

/// <summary>
/// Parent publication information
/// </summary>
public class ParentPublication
{
    /// <summary>
    /// Name of the parent publication
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Parent publication type
    ///
    /// http://uri.suomi.fi/codelist/research/Emojulkaisuntyyppi
    /// </summary>
    public ParentPublicationType? Type { get; set; }

    /// <summary>
    /// Parent publication publisher
    /// </summary>
    public string? Publisher { get; set; }
}