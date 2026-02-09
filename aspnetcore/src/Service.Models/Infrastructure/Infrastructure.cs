using System.Net.Sockets;
using Nest;

namespace CSC.PublicApi.Service.Models.Infrastructure
{
    /// <summary>Research infrastructure</summary>
    public partial class Infrastructure
    {
        /// <summary></summary>
        public long ExportSortId { get; set; }

        ///<summary>Local identifier, handled in InfrastructureIndexRepository</summary>
        [Ignore]
        public string? LocalIdentifier { get; set; }

        /// <summary>Pids, handled in InfrastructureIndexRepository</summary>
        [Ignore]
        public List<PersistentIdentifier>? Pids { get; set; }

        /// <summary>Infra identifier</summary>
        public Identifier? InfraIdentifier { get; set; }

        /// <summary>Acronym</summary>
        [Keyword]
        public string? Acronym { get; set; }

        /// <summary>Infra name</summary>
        [Nested]
        public List<DescriptiveText>? InfraName { get; set; }

        /// <summary>Organization - responsible</summary>
        [Nested]
        public ResearchOrganization? ResponsibleOrganization { get; set; }

        /// <summary>
        /// Classification - ESFRI. https://uri.suomi.fi/codelist/research/ESFRI-Domain
        /// </summary>
        [Nested]
        public List<ReferenceData>? Esfri { get; set; }

        /// <summary>Infra start date</summary>
        [Nested]
        public InfraDate? InfraStartsOn { get; set; }

        /// <summary>Infra description</summary>
        [Nested]
        public List<DescriptiveText>? InfraDescription { get; set; }

        /// <summary>
        /// Classification - field of science. https://uri.suomi.fi/codelist/research/Tieteenala2010
        /// </summary>
        [Nested]
        public List<ReferenceData>? FieldOfScience { get; set; }

        /// <summary>Has a service</summary>
        public List<InfrastructureService>? InfraServices { get; set; }

        /// <summary>Belongs to infrastructure network</summary>
        public List<InfrastructureNetwork>? InfraNetwork { get; set; }

        /// <summary>Organization - participant</summary>
        [Nested]
        public List<ResearchOrganization>? OrganizationParticipatesInfrastructure { get; set; }

        /// <summary>Infra end date</summary>
        [Nested]
        public InfraDate? InfraEndsOn { get; set; }

        /// <summary>Roadmap for Finnish Research for infrastructures</summary>
        public bool? FinlandRoadmapInfrastructure { get; set; }

        /// <summary>Infra homepage</summary>
        public List<Weblink>? InfraHomepage { get; set; }

        /// <summary>Infra contact information</summary>
        [Nested]
        public List<ContactInformation>? InfraContactInformation { get; set; }

        /// <summary>Infra research.fi URL</summary>
        public LanguageVariant? InfraResearchfiURL { get; set; }
    }

    /// <summary>Contact information</summary>
    public partial class ContactInformation
    {
        /// <summary>Contact information label</summary>
        public string? ContactInformationLabel { get; set; }

        /// <summary>Phone</summary>
        public List<string>? Phone { get; set; }

        /// <summary>Email</summary>
        public List<string>? Email { get; set; }

        /// <summary>Visiting address</summary>
        public Address? VisitingAddress { get; set; }
    }

    /// <summary>Address</summary>
    public partial class Address
    {
        /// <summary>Premise identifier</summary>
        public string? Premise { get; set; }

        /// <summary>Country - https://uri.suomi.fi/codelist/jhs/valtio_1_20120101</summary>
        public ReferenceData? Country { get; set; }

        /// <summary>Street name</summary>
        public string? Street { get; set; }

        /// <summary>Postal code</summary>
        public string? PostalCode { get; set; }

        /// <summary>Locality name</summary>
        public LanguageVariant? Locality { get; set; }
    }

    /// <summary>Identifier</summary>
    public partial class Identifier
    {
        /// <summary>Local identifier</summary>
        [Keyword]
        public string? LocalIdentifier { get; set; }

        /// <summary>Other persistent identifier</summary>
        [Nested]
        public List<PidAttributes>? OtherPid { get; set; }

        /// <summary>Persistent identifier [URN]</summary>
        [Keyword]
        public string? PersistentIdentifierURN { get; set; }

        /// <summary>persistent identifier [URN] link. https://urn.fi/ +[URN]</summary>
        public string? PersistentIdentifierURNLink { get; set; }
    }

    /// <summary>Other persistent identifier</summary>
    public partial class PidAttributes
    {
        /// <summary>Persistent identifier</summary>
        [Keyword]
        public string? Pid { get; set; }

        /// <summary>Persistent identifier type</summary>
        public string? PidType { get; set; }
    }

    /// <summary>International network infra</summary>
    public partial class InternationalInfra
    {
        /// <summary>International infra identifier</summary>
        public string? InternationalInfraIdentifier { get; set; }

        /// <summary>International infra name</summary>
        public string? InternationalInfraName { get; set; }

        /// <summary>International infra weblink</summary>
        public string? InternationalInfraWeblink { get; set; }
    }

    /// <summary>Content description text</summary>
    public partial class DescriptiveText
    {
        /// <summary>Text content</summary>
        public string? TextContent { get; set; }

        /// <summary>Text language code</summary>
        public string? TextLanguage { get; set; }
    }

    /// <summary>Reference data</summary>
    public partial class ReferenceData
    {
        /// <summary>Code description</summary>
        public LanguageVariant? CodeDescription { get; set; }

        /// <summary>Code value</summary>
        [Keyword]
        public string? CodeValue { get; set; }
    }

    /// <summary>Infrastructure network</summary>
    public partial class InfrastructureNetwork
    {
        /// <summary>Related international infra</summary>
        public InternationalInfra? RelationToInternationalInfra { get; set; }

        /// <summary>Relation type - http://uri.suomi.fi/codelist/research/relationTypes_infrastructure</summary>
        public ReferenceData? InfranetworkRelationType { get; set; }

        /// <summary>Relation valid</summary>
        public bool? RelationValid { get; set; }

        /// <summary>Related infra</summary>
        public Identifier? RelationToInfra { get; set; }

        /// <summary>Pids, handled in InfrastructureServiceIndexRepository</summary>
        [Ignore]
        public List<PersistentIdentifier>? Pids { get; set; }
    }

    /// <summary>Research organization</summary>
    public partial class ResearchOrganization
    {
        public int? DimOrganizationId { get; set; }

        /// <summary>Organization name</summary>
        public LanguageVariant? OrganizationName { get; set; }

        /// <summary>Organization identifiers</summary>
        [Nested]
        public List<PidAttributes>? OrganizationIdentifier { get; set; }
    }
    
    /// <summary>Language variant text</summary>
    public partial class LanguageVariant
    {
        /// <summary>in Finnish</summary>
        public string? Fi { get; set; }

        /// <summary>in Swedish</summary>
        public string? Sv { get; set; }

        /// <summary>in English</summary>
        public string? En { get; set; }
    }

    /// <summary>Weblink</summary>
    public partial class Weblink
    {
        /// <summary>Weblink language</summary>
        public string? WeblinkLanguage { get; set; }

        /// <summary>Weblink URL</summary>
        public string? WeblinkURL { get; set; }
    }

    /// <summary>Infrastructure's service</summary>
    public partial class InfrastructureService
    {
        ///<summary>Local identifier, handled in InfrastructureIndexRepository</summary>
        [Ignore]
        public string? LocalIdentifier { get; set; }

        /// <summary>Pids, handled in InfrastructureIndexRepository</summary>
        [Ignore]
        public List<PersistentIdentifier>? Pids { get; set; }
        
        /// <summary>Classification - service user role. https://uri.suomi.fi/codelist/research/infrapalvelu-kayttaja</summary>
        public List<ReferenceData>? ServiceUserRole { get; set; }

        /// <summary>Service end user guide</summary>
        public List<Weblink>? ServiceEndUserGuide { get; set; }

        /// <summary>Service name</summary>
        public List<DescriptiveText>? ServiceName { get; set; }

        /// <summary>Service homepage</summary>
        public List<Weblink>? ServiceHomepage { get; set; }

        /// <summary>Service identifier</summary>
        public Identifier? ServiceIdentifier { get; set; }

        /// <summary>Service starts on</summary>
        public InfraDate? ServiceStartsOn { get; set; }

        /// <summary>Service obtain instruction</summary>
        public List<DescriptiveText>? ServiceObtain { get; set; }

        /// <summary>Service ends on</summary>
        public InfraDate? ServiceEndsOn { get; set; }

        /// <summary>Service booking link</summary>
        public List<Weblink>? ServiceBookingLink { get; set; }

        /// <summary>Service contact information</summary>
        public List<ContactInformation>? ServiceContactInformation { get; set; }

        /// <summary>Classification - service target segment. https://uri.suomi.fi/codelist/research/infrapalvelu-kohderyhma"</summary>
        public List<ReferenceData>? ServiceTargetSegment { get; set; }

        /// <summary>Service research.fi URL</summary>
        public LanguageVariant? ServiceResearchfiURL { get; set; }

        /// <summary>Service description</summary>
        public List<DescriptiveText>? ServiceDescription { get; set; }

        /// <summary>Service privacy policy</summary>
        public List<Weblink>? ServicePrivacyPolicy { get; set; }

        /// <summary>Service terms of use</summary>
        public List<Weblink>? ServiceTermsOfUse { get; set; }
    }

    /// <summary>Date</summary>
    public partial class InfraDate
    {
        public InfraDate(int? year, int? month, int? day)
        {
            Year = year != null && year > 0 ? year : null;
            Month = month != null && month >= 1 && month <= 12 ? month : null;
            Day = day != null && day >= 1 && day <= 31 ? day : null;
            DatePartial = null;
            
            if (Year != null && Month != null && Day != null)
            {
                DatePartial = $"{Year}-{Month.Value.ToString("D2")}-{Day.Value.ToString("D2")}";
            }
            else if (Year != null && Month != null)
            {
                DatePartial = $"{Year}-{Month.Value.ToString("D2")}";
            }
            else if (Year != null)
            {
                DatePartial = $"{Year}";
            }
        }

        /// <summary>Day</summary>
        public int? Day { get; set; }

        /// <summary>Month</summary>
        public int? Month { get; set; }

        /// <summary>Year</summary>
        public int? Year { get; set; }

        /// <summary>
        /// Year, Month, Day: YYYY-MM-DD : "2025-11-20"
        /// Year, Month: YYYY-MM: "2025-11"
        /// Year Only: YYYY: "2025"
        /// </summary>
        public string? DatePartial { get; set; }
    }
}