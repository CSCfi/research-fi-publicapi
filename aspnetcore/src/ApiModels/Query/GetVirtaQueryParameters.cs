namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit organisaatioiden hakemiseen.
/// </summary>
public class GetVirtaQueryParameters 
{
    /// <summary>
    /// Organisaation tunnus. Valinnainen. Oppilaitosnumero/virastotunnus. Jos ei löydy kumpaakaan niin y-tunnus ilman väliviivaa
    /// </summary>
    public string? organisaatiotunnus { get; set; }
}