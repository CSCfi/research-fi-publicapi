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
    /// https://uri.suomi.fi/codelist/research/julkaisumuoto
    /// </summary>
    public PublicationFormat? Format { get; set; }

    /// <summary>
    /// Peer reviewed
    /// </summary>
    public PeerReviewed? PeerReviewed { get; set; }

    /// <summary>
    /// Target audience of the publication
    ///
    /// https://uri.suomi.fi/codelist/research/julkaisunyleiso
    /// </summary>
    public TargetAudience? TargetAudience { get; set; }

    /// <summary>
    /// National classification for publications
    ///
    /// https://uri.suomi.fi/codelist/research/Julkaisutyyppiluokitus
    /// </summary>
    public PublicationType? Type { get; set; }

     /// <summary>
     /// Thesis type
     ///
     /// https://uri.suomi.fi/codelist/research/Opinnaytetyyppi
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
    /// https://uri.suomi.fi/codelist/research/AvoinSaatavuusKytkin
    /// </summary>
    public OpenAccess OpenAccess { get; set; }

    /// <summary>
    /// Open Access status of a publication channel
    ///
    /// https://uri.suomi.fi/codelist/research/JulkaisuKanavaOA
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
    /// https://uri.suomi.fi/codelist/research/julkaisufoorumiluokitus
    /// </summary>
    public JufoClass? JufoClass { get; set; }

    /// <summary>
    /// Publication forum (recorded)
    /// </summary>
    public string? JufoCodeRecorded { get; set; }

    /// <summary>
    /// Publication forum classification (recorded)
    ///
    /// https://uri.suomi.fi/codelist/research/julkaisufoorumiluokitus
    /// </summary>
    public JufoClass? jufoClassRecorded { get; set; }

    /// <summary>
    /// DOI
    /// </summary>
    public string? Doi { get; set; }

    /// <summary>
    /// DOI handle
    /// </summary>
    public string? DoiHandle { get; set; }

    /// <summary>
    /// Self archived code
    /// </summary>
    public bool? SelfArchivedCode { get; set; }

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
    /// https://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public List<FieldOfScience>? FieldsOfScience { get; set; }

    /// <summary>
    /// Fields of art
    ///
    /// https://uri.suomi.fi/codelist/research/Taiteenala
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
    /// https://uri.suomi.fi/codelist/jhs/valtio_1_20120101
    /// </summary>
    public CountryCode? Country { get; set; }

    /// <summary>
    /// Language of the publication
    ///
    /// https://uri.suomi.fi/codelist/research/languages
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
    /// https://uri.suomi.fi/codelist/research/Artikkelintyyppikoodi
    /// </summary>
    public ArticleType? ArticleType { get; set; }

    /// <summary>
    /// Publication status code
    ///
    /// https://uri.suomi.fi/codelist/research/julkaisuntila
    /// </summary>
    public PublicationStatus? Status { get; set; }

    /// <summary>
    /// License
    /// 
    /// https://koodistot.suomi.fi/codescheme;registryCode=research;schemeCode=Lisenssi
    /// </summary>
    public LicensePublication? License { get; set; }

    /// <summary>
    /// Art publications type category
    ///
    /// https://uri.suomi.fi/codelist/research/TaidejulkaisuTyyppikategoria
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
    /// Combined information of co-publications based on national publication data collection 
    /// </summary>
    public string? CoPublicationID { get; set; }

    /// <summary>
    /// Publication information sent by individual organisations related to co-publications as part of national publication data collection
    /// </summary>
    public List<String>? OrgPublicationIDs { get; set; }

    /// <summary>
    /// Publication information sent by individual organisations related to co-publications as part of national publication data collection
    /// </summary>
    public List<OrganizationPartOfCoPublication>? OrganizationPartsOfCoPublication { get; set; }

    /// <summary>
    /// Researchfi portal URL
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }
}