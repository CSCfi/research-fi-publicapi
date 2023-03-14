namespace CSC.PublicApi.Interface.Models.Publication;

/// <summary>
/// Julkaisun avainsana
/// </summary>
public class Keyword
{
    /// <summary>
    /// Avainsana
    /// </summary>
    public string? Value { get; set; }
    
    /// <summary>
    /// Avainsanan kieli
    /// </summary>
    public string? Language { get; set; }
}