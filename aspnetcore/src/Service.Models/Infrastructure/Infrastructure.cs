// https://tietomallit.suomi.fi/model/researchfi_publicapi_infra?ver=1.0.0

namespace CSC.PublicApi.Service.Models.Infrastructure
{
    /// <summary></summary>
    public partial class Infrastructure
    {
        /// <summary></summary>
        public long ExportSortId { get; set; }
        /*
                /// <summary></summary>
                public ContactInformation? ContactInformation { get; set; }
        */
        /// <summary></summary>
        public ContentDescriptionText? DescriptiveText { get; set; }
        /// <summary></summary>
        public Identifier? InfraIdentifier { get; set; }
        /// <summary></summary>
        public InternationalNetworkInfra? InternationalInfra { get; set; }
        /// <summary></summary>
        public DescriptiveTerm? Keyword { get; set; }
        /// <summary></summary>
        public OtherPersistentIdentifier? PidAttributes { get; set; }
        /// <summary></summary>
        public ReferenceData? ReferenceData { get; set; }

        /// <summary></summary>
        public List<InfrastructureNetwork>? RelationFrom { get; set; }
        /// <summary></summary>
        public ResearchOrganization? ResearchOrganization { get; set; }
        /// <summary></summary>
        public InfrastructureSService? Service { get; set; }
        /// <summary></summary>
        public Weblink? Weblink { get; set; }

        /// <summary></summary>
        public string? Acronym { get; set; }
        /// <summary></summary>
        public List<ResearchOrganization>? ContributionOrganization { get; set; }

        /// <summary>
        /// http://uri.suomi.fi/codelist/research/ESFRI-Domain
        /// </summary>
        public List<ReferenceData>? Esfri { get; set; }

        /// <summary>
        /// http://uri.suomi.fi/codelist/research/Tieteenala2010
        /// </summary>
        public List<ReferenceData>? FieldOfScience { get; set; }

        /// <summary></summary>
        public bool? FinlandRoadmapInfrastructure { get; set; }
        /// <summary></summary>
        public List<ContactInformation>? InfraContactInformation { get; set; }
        /// <summary></summary>
        public List<ContentDescriptionText>? InfraDescription { get; set; }
        /// <summary></summary>
        public List<Weblink>? InfraHomepage { get; set; }
        /// <summary></summary>
        public List<DescriptiveTerm>? InfraKeyword { get; set; }
        /// <summary></summary>
        public Dictionary<string, object>? InfraLocalIdentifier { get; set; }
        /// <summary></summary>
        public List<ContentDescriptionText>? InfraName { get; set; }
        /// <summary></summary>
        public List<ContentDescriptionText>? InfraScientificDescription { get; set; }
        /// <summary></summary>
        public List<InfrastructureSService>? IsComposedOf { get; set; }
        /// <summary></summary>
        public ResearchOrganization? ResponsibleOrganization { get; set; }
        /// <summary></summary>
        public int? StartingYear { get; set; }
        /// <summary></summary>
        public Weblink? TermsOfUse { get; set; }

        /// <summary></summary>
        public string? ResearchFiLandingPage { get; set; }
    }

    /// <summary></summary>
    public partial class ContactInformation
    {
        /// <summary></summary>
        public string? ContactInformationContent { get; set; }
        /// <summary></summary>
        public string? ContactInformationLabel { get; set; }
        /// <summary></summary>
        public ContactInformationType? ContactInformationType { get; set; }
    }

    /// <summary></summary>
    public partial class ContentDescriptionText
    {
        /// <summary></summary>
        public string? Desciptive { get; set; }
        /// <summary></summary>
        public LanguageCode? LanguageCodeVariant { get; set; }
    }

    /// <summary></summary>
    public partial class Identifier
    {
        /// <summary></summary>
        public string? InfraLocalIdentifier { get; set; }
        /// <summary></summary>
        public List<OtherPersistentIdentifier>? OtherPid { get; set; }
        /// <summary></summary>
        public string? PersistentIdentifierUrn { get; set; }
    }

    /// <summary></summary>
    public partial class OtherPersistentIdentifier
    {
        /// <summary></summary>
        public string? Pid { get; set; }
        /// <summary></summary>
        public string? PidType { get; set; }
    }

    /// <summary></summary>
    public partial class InternationalNetworkInfra
    {
        /// <summary></summary>
        public string? InternationalInfraIdentifier { get; set; }
        /// <summary></summary>
        public string? InternationalInfraName { get; set; }
        /// <summary></summary>
        public string? InternationalInfraWeblink { get; set; }
    }

    /// <summary></summary>
    public partial class DescriptiveTerm
    {
        /// <summary></summary>
        public string? DescriptiveTermDescriptiveTerm { get; set; }
        /// <summary></summary>
        public string? LanguageCodeKeyword { get; set; }
    }

    /// <summary>
    /// http://uri.suomi.fi/codelist/research/ESFRI-Domain
    ///
    /// http://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public partial class ReferenceData
    {
        /// <summary></summary>
        public string? CodeDescriptionEn { get; set; }
        /// <summary></summary>
        public string? CodeDescriptionFi { get; set; }
        /// <summary></summary>
        public string? CodeDescriptionSv { get; set; }
        /// <summary></summary>
        public string? CodeValue { get; set; }
    }

    /// <summary>
    /// one, and only one, of related infra is given
    /// </summary>
    public partial class InfrastructureNetwork
    {
        /// <summary></summary>
        public string? RelationEndDate { get; set; }
        /// <summary></summary>
        public string? RelationStartDate { get; set; }
        /// <summary></summary>
        public Identifier? RelationToInfra { get; set; }
        /// <summary></summary>
        public InternationalNetworkInfra? RelationToInternationalInfra { get; set; }
        /// <summary></summary>
        public string? RelationType { get; set; }
        /// <summary></summary>
        public bool? RelationValid { get; set; }
    }



    /// <summary></summary>
    public partial class ResearchOrganization
    {
        /// <summary></summary>
        public string? OrganizationIdentifier { get; set; }
    }

    /// <summary></summary>
    public partial class Weblink
    {
        /// <summary></summary>
        public string? LanguageCodeWeblink { get; set; }
        /// <summary></summary>
        public string? Url { get; set; }
        /// <summary></summary>
        public string? WeblinkLabel { get; set; }
    }

    /// <summary></summary>
    public partial class InfrastructureSService
    {
        /// <summary></summary>
        public List<ContactInformation>? ServiceContactInformation { get; set; }
        /// <summary></summary>
        public List<ContentDescriptionText>? ServiceDescription { get; set; }
        /// <summary></summary>
        public List<Weblink>? ServiceHomepage { get; set; }
        /// <summary></summary>
        public Identifier? ServiceIdentifier { get; set; }
        /// <summary></summary>
        public List<ContentDescriptionText>? ServiceName { get; set; }
    }

    /// <summary></summary>
    public enum ContactInformationType
    {
        /// <summary></summary>
        Email,
        /// <summary></summary>
        Phone,
        /// <summary></summary>
        PostalAddress,
        /// <summary></summary>
        VisitingAddress
    };

    /// <summary></summary>
    public enum LanguageCode
    {
        /// <summary></summary>
        En,
        /// <summary></summary>
        Fi,
        /// <summary></summary>
        Sv
    };
}


/*
public class Infrastructure
{
    public long ExportSortId { get; set; }

    /// <summary>
    /// Infrastructure URN, key identifier in Research.fi
    /// </summary>
    public string? InfrastructureUrn { get; set; }

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
*/