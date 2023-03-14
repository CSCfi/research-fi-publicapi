namespace CSC.PublicApi.Interface.Models;

/// <summary>
/// Koodisto
/// </summary>
public class ReferenceData
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
    /// Codes name
    /// </summary>
    public string? NameEn { get; set; }
}