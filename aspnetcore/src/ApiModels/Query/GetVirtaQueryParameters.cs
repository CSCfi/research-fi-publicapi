namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit organisaatioiden hakemiseen.
/// </summary>
public class GetVirtaQueryParameters 
{
    /// <summary>
    /// Organisaation tunnus. Oppilaitosnumero tai virastotunnus. Jos organisaatiolla ei ole edellämainittuja, käytä y-tunnusta ilman väliviivaa (y-tunnus 1234567-1 muodossa 12345671)
    /// </summary>
    public string? organisaatiotunnus { get; set; }
}