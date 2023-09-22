namespace ResearchFi.CodeList;

/// <summary>
/// Code List
/// </summary>
public abstract class CodeList
{
    /// <summary>
    /// Code
    /// </summary>
    public string? Code { get; set; }
    
    /// <summary>
    /// Name of the code list in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Name of the code list in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the code list in English
    /// </summary>
    public string? NameEn { get; set; }
}