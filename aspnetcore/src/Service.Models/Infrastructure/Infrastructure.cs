namespace CSC.PublicApi.Service.Models.Infrastructure;


public class Infrastructure
{
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
    public bool? OnTheAcademyOfFinlandRoadmap { get; set; }

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
}

/// <summary>fi / en / sv</summary>
public partial class DescriptiveItemStruct
{
    public string DescriptiveItem { get; set; }
    public string DescriptiveItemLanguage { get; set; }
    public string DescriptiveItemTypeName { get; set; }
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