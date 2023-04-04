using ResearchFi.CodeList;

namespace ResearchFi.FundingCall;

/// <summary>
/// Rahoitushaku
/// </summary>
public class FundingCall
{
    /// <summary>
    /// Rahoitushaun nimi
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Utlysningens namn
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Name of the funding call
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Rahoitushaun kuvaus
    /// </summary>
    public string? DescriptionFi { get; set; }

    /// <summary>
    /// Utlysningens beskrivning
    /// </summary>
    public string? DescriptionSv { get; set; }

    /// <summary>
    /// Description of the funding call
    /// </summary>
    public string? DescriptionEn { get; set; }

    /// <summary>
    /// Hakuohjeet
    /// </summary>
    public string? ApplicationTermsFi { get; set; }

    /// <summary>
    /// Sökanvisningar
    /// </summary>
    public string? ApplicationTermsSv { get; set; }

    /// <summary>
    /// Application guidelines
    /// </summary>
    public string? ApplicationTermsEn { get; set; }

    /// <summary>
    /// Jatkuva haku
    /// </summary>
    public bool ContinuosApplication { get; set; }

    /// <summary>
    /// Haku alkaa
    /// </summary>
    public DateTime? CallProgrammeOpenDate { get; set; }

    /// <summary>
    /// Haku päättyy
    /// </summary>
    public DateTime? CallProgrammeDueDate { get; set; }

    /// <summary>
    /// Rahoitushaun verkkosivu
    /// </summary>
    public string? ApplicationUrlFi { get; set; }

    /// <summary>
    /// Utlysningens webbsida
    /// </summary>
    public string? ApplicationUrlSv { get; set; }

    /// <summary>
    /// Webpage of the funding call
    /// </summary>
    public string? ApplicationUrlEn { get; set; }

    /// <summary>
    /// Rahoittajat
    /// </summary>
    public List<Foundation>? Foundations { get; set; }

    /// <summary>
    /// Rahoitushakujen hakualat.
    /// 
    /// Koodisto: http://uri.suomi.fi/codelist/research/auroran_alat
    /// </summary>
    public List<Category>? Categories { get; set; }

    /// <summary>
    /// Yhteystiedot
    /// </summary>
    public string? ContactInformation { get; set; }
}