namespace CSC.PublicApi.Service.Models.Infrastructure;


public class Infrastructure
{
    public long ExportSortId { get; set; }

    /// <summary>
    /// Abbreviation.
    /// </summary>
    public string? Abbreviation { get; set; }

    /// <summary>
    /// Starting year.
    /// </summary>
    public int? StartingYear { get; set; }

    /// <summary>
    /// On the Academy of Finland roadmap.
    /// </summary>
    public bool? FinlandRoadmap { get; set; }

    /// <summary>
    /// Names of the infrastructure.
    /// </summary>
    public List<DescriptiveItemStruct>? InfraName { get; set; }

    /// <summary>
    /// Descriptions of the infrastructure.
    /// </summary>
    public List<DescriptiveItemStruct>? InfraDescription { get; set; }

    /// <summary>
    /// Scientific descriptions of the infrastructure.
    /// </summary>
    public List<DescriptiveItemStruct>? InfraScientificDescription { get; set; }

    /// <summary>
    /// ESFRI code of the infrastructure.
    /// </summary>
    public List<ReferenceData>? EsfriCode { get; set; }

    /// <summary>
    /// Landing page for the infrastructure on Research.fi.
    /// </summary>
    public string? ResearchFiLandingPage { get; set; }

    /// <summary>
    /// Keywords associated with the infrastructure.
    /// </summary>
    public List<Keywords>? Keywords { get; set; }

    /// <summary>
    /// Homepage links for the infrastructure.
    /// </summary>
    public List<Weblink>? InfraHomepage { get; set; }

    /// <summary>
    /// Terms of use for the infrastructure.
    /// </summary>
    public List<Weblink>? InfraTermsOfUse { get; set; }

    /// <summary>
    /// Service belonging to infrastructure
    /// </summary>
    public List<ResearchInfrastructureService>? InfrastructuresService { get; set; }

    /// <summary>
    /// Contact information
    /// </summary>
    public List<ContactInformation>? DimInfrastructureId1 { get; set; }
}

/// <summary>fi / en / sv</summary>
public partial class DescriptiveItemStruct
{
    public string? DescriptiveItem { get; set; }
    public string? DescriptiveItemLanguage { get; set; }
    public string? DescriptiveItemTypeName { get; set; }
}

/// <summary>
/// Represents keywords with language and content.
/// </summary>
public partial class Keywords
{
    /// <summary>
    /// Language of the keyword.
    /// </summary>
    public string? DimKeywordLanguage { get; set; }

    /// <summary>
    /// Content of the keyword.
    /// </summary>
    public string? Keyword { get; set; }
}

/// <summary>
/// Represents a web link with language variant, label, and URL.
/// </summary>
public partial class Weblink
{
    /// <summary>
    /// Language variant of the web link.
    /// </summary>
    public string? LanguageVariant { get; set; }

    /// <summary>
    /// Label for the web link.
    /// </summary>
    public string? LinkLabel { get; set; }

    /// <summary>
    /// URL of the web link.
    /// </summary>
    public string? WebLinkUrl { get; set; }
}

/// <summary></summary>
public partial class ResearchInfrastructureService
{
    /// <summary></summary>
    public List<ContactInformation>? DimServiceId1 { get; set; }
    /// <summary></summary>
    public List<ServiceOtherIdentifier>? OtherIdentifierService { get; set; }

    /// <summary>service's URN, key identifier in Research.fi</summary>
    public string? ServiceUrn { get; set; }

    /// <summary>fi / en / sv</summary>
    public List<DescriptiveItemStruct>? ServiceDescription { get; set; }

    /// <summary>fi / en / sv</summary>
    public List<DescriptiveItemStruct>? ServiceName { get; set; }
    /// <summary></summary>
    public List<Weblink>? ServiceWebsite { get; set; }
}

/// <summary></summary>
public partial class ContactInformation
{
    /// <summary></summary>
    public string? ContactInformationContent { get; set; }
    /// <summary></summary>
    public string? ContactInformationType { get; set; }
    /// <summary></summary>
    public string? ContactName { get; set; }
}

/// <summary></summary>
public partial class ServiceOtherIdentifier
{
    /// <summary></summary>
    public string? PidContentService { get; set; }
    /// <summary></summary>
    public EIdentifierType? PidTypeService { get; set; }
}

/// <summary>Organisaation paikallinen tunniste infrastruktuurille</summary>
public enum EIdentifierType {
    /// <summary></summary>
    LocalIdentifier
};