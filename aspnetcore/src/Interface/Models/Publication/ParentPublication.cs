namespace CSC.PublicApi.Interface.Models.Publication;

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
    /// </summary>
    public ReferenceData? Type { get; set; }

    /// <summary>
    /// Emojulkaisun toimittajat
    /// </summary>
    public string? Publisher { get; set; }
}