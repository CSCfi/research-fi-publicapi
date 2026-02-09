using Nest;

namespace CSC.PublicApi.Service.Models.Infrastructure
{
    /// <summary>Infrastructure's service</summary>
    public partial class Service
    {
        /// <summary></summary>
        public long ExportSortId { get; set; }

        ///<summary>Local identifier, handled in InfrastructureIndexRepository</summary>
        [Ignore]
        public string? LocalIdentifier { get; set; }

        /// <summary>Pids, handled in InfrastructureServiceIndexRepository</summary>
        [Ignore]
        public List<PersistentIdentifier>? Pids { get; set; }

        /// <summary>Classification - service user role. https://uri.suomi.fi/codelist/research/infrapalvelu-kayttaja</summary>
        public List<ReferenceData>? ServiceUserRole { get; set; }

        /// <summary>Service end user guide</summary>
        public List<Weblink>? ServiceEndUserGuide { get; set; }

        /// <summary>Service is part of infrastructure</summary>
        [Nested]
        public ServiceInfrastructure? IsPartOfInfrastructure { get; set; }

        /// <summary>Service name</summary>
        [Nested]
        public List<DescriptiveText>? ServiceName { get; set; }

        /// <summary>Service homepage</summary>
        public List<Weblink>? ServiceHomepage { get; set; }

        /// <summary>Service identifier</summary>
        public Identifier? ServiceIdentifier { get; set; }

        /// <summary>Service starts on</summary>
        [Nested]
        public InfraDate? ServiceStartsOn { get; set; }

        /// <summary>Service obtain instruction</summary>
        public List<DescriptiveText>? ServiceObtain { get; set; }

        /// <summary>Service ends on</summary>
        [Nested]
        public InfraDate? ServiceEndsOn { get; set; }

        /// <summary>Service booking link</summary>
        public List<Weblink>? ServiceBookingLink { get; set; }

        /// <summary>Classification - service target segment. https://uri.suomi.fi/codelist/research/infrapalvelu-kohderyhma"</summary>
        public List<ReferenceData>? ServiceTargetSegment { get; set; }

        /// <summary>Service research.fi URL</summary>
        public LanguageVariant? ServiceResearchfiURL { get; set; }

        /// <summary>Service contact information</summary>
        public List<ContactInformation>? ServiceContactInformation { get; set; }

        /// <summary>Service description</summary>
        [Nested]
        public List<DescriptiveText>? ServiceDescription { get; set; }

        /// <summary>Service privacy policy</summary>
        public List<Weblink>? ServicePrivacyPolicy { get; set; }

        /// <summary>Service terms of use</summary>
        public List<Weblink>? ServiceTermsOfUse { get; set; }

        /// <summary>DimInfrastructureId</summary>
        [Ignore]
        public int DimInfrastructureId { get; set; }
    }

    /// <summary>Research infrastructure</summary>
    public partial class ServiceInfrastructure
    {
        /// <summary>Pids, handled in InfrastructureServiceIndexRepository</summary>
        [Ignore]
        public List<PersistentIdentifier>? Pids { get; set; }

        /// <summary>Infrastructure identifier</summary>
        public Identifier? InfraIdentifier { get; set; }

        /// <summary>Organization - responsible</summary>
        public ResearchOrganization? ResponsibleOrganization { get; set; }

        /// <summary>Infrastructure name</summary>
        public List<DescriptiveText>? InfraName { get; set; }

        /// <summary>
        /// Classification - ESFRI. https://uri.suomi.fi/codelist/research/ESFRI-Domain
        /// </summary>
        public List<ReferenceData>? Esfri { get; set; }

        /// <summary>Infrastructure research.fi URL</summary>
        public LanguageVariant? ResearchfiURL { get; set; }
    }
}