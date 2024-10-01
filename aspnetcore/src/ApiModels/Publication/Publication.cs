using Microsoft.VisualBasic;
using ResearchFi.CodeList;
using OpenAccess = ResearchFi.CodeList.OpenAccess;

namespace ResearchFi.Publication;

/// <summary>
/// Publication
/// </summary>
public class Publication
{
    /// <summary>
    /// Publication ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Julkaisun organisaatiotunnus
    /// </summary>
    public string? OriginalPublicationId { get; set; }
    
    /// <summary>
    /// Name of the publication
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Publication year
    /// </summary>
    public string? PublicationYear { get; set; }
    
    /// <summary>
    /// Reporting year
    /// </summary>
    public string? ReportingYear { get; set; }

    /// <summary>
    /// Authors of the publication as text
    /// </summary>
    public string? AuthorsText { get; set; }

    /// <summary>
    /// Publication organizations
    /// </summary>
    public List<Organization>? Organizations { get; set; }    
    
    /// <summary>
    /// Authors of the publication
    /// </summary>
    public List<Author>? Authors { get; set; }

    /// <summary>
    /// Publication format
    ///
    /// http://uri.suomi.fi/codelist/research/julkaisumuoto
    /// </summary>
    public PublicationFormat? Format { get; set; }

    /// <summary>
    /// Peer reviewed
    /// </summary>
    public PeerReviewed? PeerReviewed { get; set; }

    /// <summary>
    /// Target audience of the publication
    ///
    /// http://uri.suomi.fi/codelist/research/julkaisunyleiso
    /// </summary>
    public TargetAudience? TargetAudience { get; set; }

    /// <summary>
    /// National classification for publications
    ///
    /// http://uri.suomi.fi/codelist/research/Julkaisutyyppiluokitus
    /// </summary>
    public PublicationType? Type { get; set; }

     /// <summary>
     /// Thesis type
     ///
     /// http://uri.suomi.fi/codelist/research/Opinnaytetyyppi
     /// </summary>
     public ThesisType? ThesisType { get; set; }

    /// <summary>
    /// Journal name
    /// </summary>
    public string? JournalName { get; set; }
    
    /// <summary>
    /// Issue number
    /// </summary>
    public string? IssueNumber { get; set; }

    /// <summary>
    /// Conference name
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
    /// Volume
    /// </summary>
    public string? Volume { get; set; }

    /// <summary>
    /// Page number
    /// </summary>
    public string? PageNumberText { get; set; }

    /// <summary>
    /// Article number
    /// </summary>
    public string? ArticleNumberText { get; set; }

    /// <summary>
    /// Open access of publication
    ///
    /// http://uri.suomi.fi/codelist/research/AvoinSaatavuusKytkin
    /// </summary>
    public OpenAccess OpenAccess { get; set; }

    /// <summary>
    /// Open Access status of a publication channel
    ///
    /// http://uri.suomi.fi/codelist/research/JulkaisuKanavaOA
    /// </summary>
    public PublisherOpenAccess PublisherOpenAccess { get; set; }

    /// <summary>
    /// Parent publication
    /// </summary>
    public ParentPublication? ParentPublication { get; set; }

    /// <summary>
    /// Publisher name
    /// </summary>
    public string? PublisherName { get; set; }

    /// <summary>
    /// Publisher location
    /// </summary>
    public string? PublisherLocation { get; set; }

    /// <summary>
    /// Publication forum
    /// </summary>
    public string? JufoCode { get; set; }

    /// <summary>
    /// Publication forum classification
    ///
    /// http://uri.suomi.fi/codelist/research/julkaisufoorumiluokitus
    /// </summary>
    public JufoClass? JufoClass { get; set; }

    /// <summary>
    /// DOI
    /// </summary>
    public string? Doi { get; set; }

    /// <summary>
    /// DOI handle
    /// </summary>
    public string? DoiHandle { get; set; }

    /// <summary>
    /// Self archived publication
    /// </summary>
    public List<LocallyReportedPublicationInformation>? SelfArchived { get; set; }
    
    /// <summary>
    /// Preprint
    /// </summary>
    public List<LocallyReportedPublicationInformation>? Preprint { get; set; }

    /// <summary>
    /// Fields of science
    ///
    /// http://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public List<FieldOfScience>? FieldsOfScience { get; set; }

    /// <summary>
    /// Fields of art
    ///
    /// http://uri.suomi.fi/codelist/research/Taiteenala
    /// </summary>
    public List<FieldOfArt>? FieldsOfArt { get; set; }

    /// <summary>
    /// Keywords
    /// </summary>
    public List<Keyword>? Keywords { get; set; }

    /// <summary>
    /// Is an international publication
    /// </summary>
    public bool? InternationalPublication { get; set; }

    /// <summary>
    /// Publication country
    ///
    /// http://uri.suomi.fi/codelist/jhs/valtio_1_20120101
    /// </summary>
    public CountryCode? Country { get; set; }

    /// <summary>
    /// Language of the publication
    ///
    /// http://uri.suomi.fi/codelist/research/languages
    /// </summary>
    public Language? Language { get; set; }

    /// <summary>
    /// Is an international collaboration
    /// </summary>
    public bool? InternationalCollaboration { get; set; }

    /// <summary>
    /// Is an business collaboration
    /// </summary>
    public bool? BusinessCollaboration { get; set; }
    
    /// <summary>
    /// Publication fee in euro
    /// </summary>
    public decimal? ApcFeeEur { get; set; }

    /// <summary>
    /// Publication fee payment year
    /// </summary>
    public string? ApcPaymentYear { get; set; }

    /// <summary>
    /// Article type code
    ///
    /// http://uri.suomi.fi/codelist/research/Artikkelintyyppikoodi
    /// </summary>
    public ArticleType? ArticleType { get; set; }

    /// <summary>
    /// Publication status code
    ///
    /// /// http://uri.suomi.fi/codelist/research/julkaisuntila
    /// </summary>
    public PublicationStatus? Status { get; set; }

    /// <summary>
    /// License
    /// 
    /// http://uri.suomi.fi/codelist/fairdata/license
    /// </summary>
    public License? License { get; set; }

    /// <summary>
    /// Art publications type category
    ///
    /// http://uri.suomi.fi/codelist/research/TaidejulkaisuTyyppikategoria
    /// </summary>
    public List<ArtPublicationTypeCategory>? ArtPublicationTypeCategory { get; set; }

    /// <summary>
    /// Abstract
    /// </summary>
    public string? Abstract { get; set; }

    /// <summary>
    /// Creation time
    /// </summary>
    public DateTime? Created { get; set; }

    /// <summary>
    /// Modification time
    /// </summary>
    public DateTime? Modified { get; set; }

    /// <summary>
    /// Osajulkaisun yhteisjulkaisu - TEMPORARY NAME
    /// </summary>
    public string? Yhteisjulkaisu { get; set; }

    /// <summary>
    /// Yhteisjulkaisuun liittyvät osajulkaisut - TEMPORARY NAME
    /// </summary>
    public List<String>? Osajulkaisut { get; set; }   
}