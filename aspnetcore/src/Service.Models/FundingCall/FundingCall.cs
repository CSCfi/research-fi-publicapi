using Nest;

namespace CSC.PublicApi.Service.Models.FundingCall;

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
    /// Hakualat
    /// </summary>
    public List<ReferenceData>? Categories { get; set; }

    /// <summary>
    /// Yhteystiedot
    /// </summary>
    public string? ContactInformation { get; set; }
    
    [Ignore]
    public int Id { get; set; }
    
    [Ignore]
    public string? Abbreviation { get; set; }
    
    [Ignore]
    public string? SourceId { get; set; }
    
    [Ignore]
    public string? SourceDescription { get; set; }

    [Ignore]
    public string? EuCallId { get; set; }

    [Ignore]
    public string? SourceProgrammeId { get; set; }
}