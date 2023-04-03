namespace ResearchFi.CodeList;

/// <summary>
/// Koodisto
/// </summary>
public abstract class CodeList
{
    /// <summary>
    /// Koodiston koodi
    /// </summary>
    public string? Code { get; set; }
    
    /// <summary>
    /// Koodin nimi
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Namn på koden
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the code
    /// </summary>
    public string? NameEn { get; set; }
}