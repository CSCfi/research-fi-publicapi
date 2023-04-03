using Microsoft.VisualBasic;
using ResearchFi.CodeList;
using OpenAccess = ResearchFi.CodeList.OpenAccess;

namespace ResearchFi.Publication;

/// <summary>
/// Julkaisu
/// </summary>
public class Publication
{
    /// <summary>
    /// Julkaisun tunnus
    /// </summary>
    public string? Id { get; set; }
    
    /// <summary>
    /// Julkaisun nimi
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Julkaisuvuosi
    /// </summary>
    public string? PublicationYear { get; set; }
    
    /// <summary>
    /// Ilmoitusvuosi 	
    /// </summary>
    public string? ReportingYear { get; set; }

    /// <summary>
    /// Tekijät
    /// </summary>
    public string? AuthorsText { get; set; }

    /// <summary>
    /// Julkaisun organisaatiot
    /// </summary>
    public List<Organization>? Organizations { get; set; }    
    
    /// <summary>
    /// Julkaisun tekij?t
    /// </summary>
    public List<Author>? Authors { get; set; }

    /// <summary>
    /// Julkaisun muoto
    ///
    /// http://uri.suomi.fi/codelist/research/julkaisumuoto
    /// </summary>
    public PublicationFormat? Format { get; set; }

    /// <summary>
    /// Vertaisarvioitu
    /// </summary>
    public PeerReviewed? PeerReviewed { get; set; }

    /// <summary>
    /// Yleisö
    ///
    /// http://uri.suomi.fi/codelist/research/julkaisunyleiso
    /// </summary>
    public TargetAudience? TargetAudience { get; set; }

    /// <summary>
    /// OKM:n julkaisutyyppiluokitus
    ///
    /// http://uri.suomi.fi/codelist/research/Julkaisutyyppiluokitus
    /// </summary>
    public PublicationType? Type { get; set; }

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
    /// ISSN
    /// </summary>
    public List<string>? Issn { get; set; }
    
    /// <summary>
    /// ISBN
    /// </summary>
    public List<string>? Isbn { get; set; }

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
    /// Julkaisun avoin saatavuus
    ///
    /// http://uri.suomi.fi/codelist/research/AvoinSaatavuusKytkin
    /// </summary>
    public OpenAccess OpenAccess { get; set; }

    /// <summary>
    /// Julkaisukanavan avoin saatavuus
    ///
    /// http://uri.suomi.fi/codelist/research/JulkaisuKanavaOA
    /// </summary>
    public PublisherOpenAccess PublisherOpenAccess { get; set; }

    /// <summary>
    /// Emojulkaisu
    /// </summary>
    public ParentPublication? ParentPublication { get; set; }

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
    /// Julkaisufoorumiluokitus
    ///
    /// http://uri.suomi.fi/codelist/research/julkaisufoorumiluokitus
    /// </summary>
    public JufoClass? JufoClass { get; set; }

    /// <summary>
    /// Pysyv? osoite
    /// </summary>
    public string? Doi { get; set; }

    /// <summary>
    /// Pysyv? osoite teksti
    /// </summary>
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
    ///
    /// http://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public List<FieldOfScience>? FieldsOfScience { get; set; }

    /// <summary>
    /// OKM:n ohjauksen ala
    ///
    /// https://wiki.eduuni.fi/display/cscsuorat/7.2+OKM%3An+ohjauksen+alat+2022
    /// </summary>
    public List<FieldOfEducation>? FieldsOfEducation { get; set; }

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
    ///
    /// http://uri.suomi.fi/codelist/jhs/valtio_1_20120101
    /// </summary>
    public CountryCode? Country { get; set; }

    /// <summary>
    /// Kieli
    ///
    /// http://uri.suomi.fi/codelist/research/languages
    /// </summary>
    public Language? Language { get; set; }

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
    public string? ApcPaymentYear { get; set; }

    /// <summary>
    /// Artikkelin tyyppi
    ///
    /// http://uri.suomi.fi/codelist/research/Artikkelintyyppikoodi
    /// </summary>
    public ArticleType? ArticleType { get; set; }

    /// <summary>
    /// Julkaisun tila
    ///
    /// /// http://uri.suomi.fi/codelist/research/julkaisuntila
    /// </summary>
    public PublicationStatus? Status { get; set; }

    /// <summary>
    /// Lisenssi
    /// 
    /// http://uri.suomi.fi/codelist/fairdata/license
    /// </summary>
    public License? License { get; set; }

    /// <summary>
    /// Taiteenala
    ///
    /// http://uri.suomi.fi/codelist/research/Taiteenala
    /// </summary>
    public List<FieldOfArts>? FieldsOfArts { get; set; }

    /// <summary>
    /// Taidealan tyyppikategoria
    ///
    /// http://uri.suomi.fi/codelist/research/TaidejulkaisuTyyppikategoria
    /// </summary>
    public List<ArtPublicationTypeCategory>? ArtPublicationTypeCategory { get; set; }

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
}