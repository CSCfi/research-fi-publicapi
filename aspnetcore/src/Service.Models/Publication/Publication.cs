using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class Publication
{
    /// <summary>
    /// Julkaisun tunnus
    /// </summary>
    [Keyword]
    public string? Id { get; set; }

    /// <summary>
    /// Julkaisun nimi
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Julkaisuvuosi
    /// </summary>
    public DateTime? PublicationYear { get; set; }

    /// <summary>
    /// Tekijäteksti
    /// </summary>
    public string? AuthorsText { get; set; }
    
    /// <summary>
    /// Organisaatiot
    /// </summary>
    public List<Organization>? Organizations { get; set; }
    
    /// <summary>
    /// Tekijät
    /// </summary>
    public List<Author>? Authors { get; set; }

    [Ignore]
    public List<FactContribution>? DatabaseContributions { get; set; }

    /// <summary>
    /// Julkaisun muoto
    /// </summary>
    public ReferenceData? Format { get; set; }

    /// <summary>
    /// Vertaisarvioitu
    /// </summary>
    public ReferenceData? PeerReviewed { get; set; }
    
    [Ignore]
    public bool? DatabasePeerReviewed { get; set; }

    /// <summary>
    /// Yleisö
    /// </summary>
    public ReferenceData? TargetAudience { get; set; }

    /// <summary>
    /// OKM:n julkaisutyyppiluokitus
    /// </summary>
    public string? TypeCode { get; set; }

    /// <summary>
    /// Lehti
    /// </summary>
    public string? JournalName { get; set; }
    
    /// <summary>
    /// Numero
    /// </summary>
    public string? IssueNumber { get; set; }

    /// <summary>
    /// Konferenssi
    /// </summary>
    public string? ConferenceName { get; set; }

    /// <summary>
    /// Julkaisuun liittyvät ISSN tunnisteet 
    /// </summary>
    [Keyword]
    public List<string>? Issn { get; set; }
    
    /// <summary>
    /// ISSN
    /// </summary>
    [Ignore]
    public string? Issn1 { get; set; }

    /// <summary>
    /// ISSN2
    /// </summary>
    [Ignore]
    public string? Issn2 { get; set; }

    /// <summary>
    /// Volyymi
    /// </summary>
    public string? Volume { get; set; }

    /// <summary>
    /// Sivut
    /// </summary>
    public string? PageNumberText { get; set; }

    /// <summary>
    /// Artikkelinumero
    /// </summary>
    public string? ArticleNumberText { get; set; }

    /// <summary>
    /// Emojulkaisu
    /// </summary>
    public ParentPublication? ParentPublication { get; set; }

    /// <summary>
    /// Julkaisuun liittyvät ISBN tunnisteet 
    /// </summary>
    [Keyword]
    public List<string>? Isbn { get; set; }
    
    /// <summary>
    /// ISBN
    /// </summary>
    [Ignore]
    public string? Isbn1 { get; set; }

    /// <summary>
    /// ISBN2
    /// </summary>
    [Ignore]
    public string? Isbn2 { get; set; }

    /// <summary>
    /// Kustantaja
    /// </summary>
    public string? PublisherName { get; set; }

    /// <summary>
    /// Kustannuspaikka
    /// </summary>
    public string? PublisherLocation { get; set; }

    /// <summary>
    /// Julkaisufoorumi
    /// </summary>
    public string? JufoCode { get; set; }

    /// <summary>
    /// Julkaisufoorumitaso
    /// </summary>
    public string? JufoClassCode { get; set; }

    /// <summary>
    /// Linkit
    /// </summary>
    [Keyword]
    public string? Doi { get; set; }

    /// <summary>
    /// Pysyvä osoite
    /// </summary>
    [Keyword]
    public string? DoiHandle { get; set; }

    /// <summary>
    /// Rinnakkaistallenne
    /// </summary>
    public List<LocallyReportedPublicationInformation>? SelfArchived { get; set; }
    
    /// <summary>
    /// Preprint
    /// </summary>
    public List<LocallyReportedPublicationInformation>? Preprint { get; set; }

    /// <summary>
    /// Tieteenalat
    /// </summary>
    public List<ReferenceData>? FieldsOfScience { get; set; }

    /// <summary>
    /// OKM:n ohjauksen ala
    /// </summary>
    public List<ReferenceData>? FieldsOfEducation { get; set; }

    /// <summary>
    /// Avainsanat
    /// </summary>
    public List<Keyword>? Keywords { get; set; }

    /// <summary>
    /// Julkaisun kansainvälisyys
    /// </summary>
    public bool? InternationalPublication { get; set; }

    /// <summary>
    /// Julkaisumaa
    /// </summary>
    [Keyword]
    public string? CountryCode { get; set; }

    /// <summary>
    /// Kieli
    /// </summary>
    [Keyword]
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Kansainvälinen yhteisjulkaisu
    /// </summary>
    public bool? InternationalCollaboration { get; set; }

    /// <summary>
    /// Yhteisjulkaisu yrityksen kanssa
    /// </summary>
    public bool? BusinessCollaboration { get; set; }

    /// <summary>
    /// Avoin saatavuus julkaisumaksu
    /// </summary>
    public decimal? ApcFeeEur { get; set; }

    /// <summary>
    /// Avoin saatavuus julkaisumaksun vuosi 
    /// </summary>
    public DateTime? ApcPaymentYear { get; set; }

    /// <summary>
    /// Artikkelin tyyppi
    /// </summary>
    public ReferenceData? ArticleTypeCode { get; set; }

    /// <summary>
    /// Ilmoitusvuosi 	
    /// </summary>
    public DateTime? ReportingYear { get; set; }
    
    /// <summary>
    /// Julkaisun tila	
    /// </summary>
    [Keyword]
    public string? StatusCode { get; set; }

    /// <summary>
    /// Lisenssikoodi
    /// </summary>
    [Keyword]
    public string? LicenseId { get; set; }

    /// <summary>
    /// Taiteenala
    /// </summary>
    public List<ReferenceData>? FieldOfArts { get; set; }

    /// <summary>
    /// TaidealanTyyppiKategoria
    /// </summary>
    public List<ReferenceData>? ArtPublicationTypeCategory { get; set; }

    /// <summary>
    /// Tiivistelmä
    /// </summary>
    public string? Abstract { get; set; }

    /// <summary>
    /// Luontiaika
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Muokkausaika
    /// </summary>
    public DateTime Modified { get; set; }

    [Ignore]
    public ReferenceData? ParentPublicationType { get; set; }

    [Ignore]
    public string? ParentPublicationName { get; set; }

    [Ignore]
    public string? ParentPublicationPublisher { get; set; }
}