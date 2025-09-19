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
        public DateTime? RelationEndDate { get; set; }
        /// <summary></summary>
        public DateTime? RelationStartDate { get; set; }
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
