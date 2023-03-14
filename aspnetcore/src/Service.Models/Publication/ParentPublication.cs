namespace CSC.PublicApi.Service.Models.Publication;

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