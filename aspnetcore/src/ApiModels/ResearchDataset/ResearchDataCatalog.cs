namespace ResearchFi.ResearchDataset;

/// <summary>
/// Datakatalogi
/// </summary>
public class ResearchDataCatalog
{
    /// <summary>
    /// Datakatalogin tunniste
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Datakatalogin nimi
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Datakatalognamn
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Data catalog name
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Lähdetunniste
    /// </summary>
    public string? SourceId { get; set; }

    /// <summary>
    /// Lähteen kuvaus
    /// </summary>
    public string? SourceDescription { get; set; }
}