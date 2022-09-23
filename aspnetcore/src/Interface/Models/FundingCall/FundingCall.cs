using System.Text.Json.Serialization;

namespace CSC.PublicApi.Interface.Models.FundingCall;

public class FundingCall
{
    /// <summary>
    /// Haun nimi
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Utlysningens namn
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Funding call's name
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Haun kuvaus
    /// </summary>
    public string? DescriptionFi { get; set; }

    /// <summary>
    /// Utlysningens beskrivning
    /// </summary>
    public string? DescriptionSv { get; set; }

    /// <summary>
    /// Description of the call
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

    [JsonPropertyName("applicationURL_fi")]
    public string? ApplicationURLFi { get; set; }

    /// <summary>
    /// Utlysningens webbsida
    /// </summary>
    [JsonPropertyName("applicationURL_sv")]
    public string? ApplicationURLSv { get; set; }

    /// <summary>
    /// Webpage of the funding call
    /// </summary>
    [JsonPropertyName("applicationURL_en")]
    public string? ApplicationURLEn { get; set; }

    /// <summary>
    /// Rahoittajat
    /// </summary>
    public Foundation[]? Foundation { get; set; }

    /// <summary>
    /// Hakualat
    /// </summary>
    public Category[]? Categories { get; set; }

    /// <summary>
    /// Yhteystiedot
    /// </summary>
    public string? ContactInformation { get; set; }
}