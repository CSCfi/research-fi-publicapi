namespace ResearchFi.Infrastructure
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a coordinate containing various infrastructure-related details.
    /// </summary>
    public partial class Coordinate
    {
        /// <summary>
        /// Gets or sets the contact information.
        /// </summary>
        public ContactInformation? DimContactInformation { get; set; }

        /// <summary>
        /// Gets or sets the descriptive item details.
        /// </summary>
        public Descriptions? DimDescriptiveItem { get; set; }

        /// <summary>
        /// Gets or sets the research infrastructure details.
        /// </summary>
        public Infrastructure? Infrastructure { get; set; }

        /// <summary>
        /// Gets or sets the keyword details.
        /// </summary>
        public Keywords? DimKeyword { get; set; }

        /// <summary>
        /// Gets or sets the service details of the infrastructure.
        /// </summary>
        public ResearchInfrastructureService? DimService { get; set; }

        /// <summary>
        /// Gets or sets the web link details.
        /// </summary>
        public Weblink? DimWebLink { get; set; }

        /// <summary>
        /// Gets or sets the organization information.
        /// </summary>
        public OrganizationInformation? OrganizationInformation { get; set; }

        /// <summary>
        /// Gets or sets the relationship to other infrastructures.
        /// </summary>
        public InfrastructureRelations? RelationshipToInfrastructure { get; set; }
    }

    /// <summary>
    /// Represents contact information.
    /// </summary>
    public partial class ContactInformation
    {
        /// <summary>
        /// Gets or sets the content of the contact information.
        /// </summary>
        public string? ContactInformationContent { get; set; }

        /// <summary>
        /// Gets or sets the type of the contact information.
        /// </summary>
        public ContactInformationType? ContactInformationType { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact person.
        /// </summary>
        public string? ContactName { get; set; }
    }

    /// <summary>
    /// Represents descriptive items.
    /// </summary>
    public partial class Descriptions
    {
        /// <summary>
        /// Gets or sets the list of descriptive items.
        /// </summary>
        public List<string>? DescriptiveItem { get; set; }

        /// <summary>
        /// Gets or sets the list of language codes for the descriptive items.
        /// </summary>
        public List<DescriptiveItemLanguageCode>? DescriptiveItemLanguage { get; set; }
    }

    /// <summary>
    /// Research infrastructure details.
    /// </summary>
    public partial class Infrastructure
    {
        /// <summary>
        /// Abbreviation.
        /// </summary>
        public string? Acronym { get; set; }

        /// <summary>
        /// Infrastructure contact information.
        /// </summary>
        public List<ContactInformation>? DimInfrastructureId1 { get; set; }

        /// <summary>
        /// ESFRI classification.
        /// </summary>
        public List<string>? EsfriCode { get; set; }

        /// <summary>
        /// Field of science.
        /// </summary>
        public List<string>? FieldOfScience { get; set; }

        /// <summary>
        /// On the Academy of Finland roadmap.
        /// </summary>
        public bool? FinlandRoadmap { get; set; }

        /// <summary>
        /// Infrastructure description.
        /// </summary>
        public Descriptions? InfraDescription { get; set; }

        /// <summary>
        /// Infrastructure weblink.
        /// </summary>
        public List<Weblink>? InfraHomepage { get; set; }

        /// <summary>
        /// Infrastructure name.
        /// </summary>
        public Descriptions? InfraName { get; set; }

        /// <summary>
        /// Infrastructure scientific description.
        /// </summary>
        public Descriptions? InfraScientificDescription { get; set; }

        /// <summary>
        /// Infrastructure local identifier.
        /// </summary>
        public string? InfrastructureLocalIdentifier { get; set; }

        /// <summary>
        /// Infrastructure PID [URN].
        /// </summary>
        public string? InfrastructureUrn { get; set; }

        /// <summary>
        /// Service belonging to infrastructure.
        /// </summary>
        public ResearchInfrastructureService? InfrastructuresService { get; set; }

        /// <summary>
        /// Infrastructure terms of use.
        /// </summary>
        public List<Weblink>? InfraTermsOfUse { get; set; }

        /// <summary>
        /// Keywords.
        /// </summary>
        public List<Keywords>? Keywords { get; set; }

        /// <summary>
        /// Organization related to infrastructure.
        /// </summary>
        public List<OrganizationInformation>? OrganizationRelatedToInfra { get; set; }

        /// <summary>
        /// Related infrastructure.
        /// </summary>
        public List<InfrastructureRelations>? RelatedInfrastructure { get; set; }

        /// <summary>
        /// Infrastruktuurin laskeutumissivu Tiede ja Tutkimus.fi -portaalissa.
        /// </summary>
        public string? ResearchFiLandingPage { get; set; }

        /// <summary>
        /// Organization responsible for infrastructure.
        /// </summary>
        public OrganizationInformation? ResponsibleOrganization { get; set; }

        /// <summary>
        /// Starting year.
        /// </summary>
        public double? StartingYear { get; set; }
    }

    /// <summary>
    /// Represents a web link.
    /// </summary>
    public partial class Weblink
    {
        /// <summary>
        /// Link language variant.
        /// </summary>
        public string? LanguageVariant { get; set; }

        /// <summary>
        /// Link label.
        /// </summary>
        public string? LinkLabel { get; set; }

        /// <summary>
        /// Link (url).
        /// </summary>
        public string? WebLinkUrl { get; set; }
    }

    /// <summary>
    /// Research infrastructure service.
    /// </summary>
    public partial class ResearchInfrastructureService
    {
        /// <summary>
        /// Service contact information.
        /// </summary>
        public List<ContactInformation>? DimServiceId1 { get; set; }

        /// <summary>
        /// Service PID [URN].
        /// </summary>
        public string? ServiceUrn { get; set; }

        /// <summary>
        /// Service description.
        /// </summary>
        public Descriptions? ServiceDescription { get; set; }

        /// <summary>
        /// Service name.
        /// </summary>
        public Descriptions? ServiceName { get; set; }

        /// <summary>
        /// Service website.
        /// </summary>
        public List<Weblink>? ServiceWebsite { get; set; }
    }

    /// <summary>
    /// Keywords.
    /// </summary>
    public partial class Keywords
    {
        /// <summary>
        /// Keyword language.
        /// </summary>
        public string? DimKeywordLanguage { get; set; }

        /// <summary>
        /// Keyword content.
        /// </summary>
        public string? Keyword { get; set; }
    }

    /// <summary>
    /// Organization information.
    /// </summary>
    public partial class OrganizationInformation
    {
        /// <summary>
        /// Organization identifier.
        /// </summary>
        public string? OrganizationIdentifier { get; set; }

        /// <summary>
        /// Organization PID type.
        /// </summary>
        public OrganizationPidType? OrganizationPidType { get; set; }
    }

    /// <summary>
    /// Infrastructure relations.
    /// </summary>
    public partial class InfrastructureRelations
    {
        /// <summary>
        /// Unidentified international infra name.
        /// </summary>
        public string? InternationalInfraName { get; set; }

        /// <summary>
        /// Unidentified international infra weblink.
        /// </summary>
        public string? InternationalInfraWeblink { get; set; }

        /// <summary>
        /// Related infra PID [URN].
        /// </summary>
        public string? RelatedInfraPid { get; set; }

        /// <summary>
        /// Related infrastructure local identifier.
        /// </summary>
        public string? RelatedInfrastructureLocalIdentifier { get; set; }

        /// <summary>
        /// Relation end date.
        /// </summary>
        public string? RelationEndDate { get; set; }

        /// <summary>
        /// Relation type code.
        /// </summary>
        public string? RelationTypeCode { get; set; }

        /// <summary>
        /// Relationship start date.
        /// </summary>
        public string? RelationshipStartDate { get; set; }

        /// <summary>
        /// Valid relationship.
        /// </summary>
        public ValidRelationship? ValidRelation { get; set; }
    }

    /// <summary>
    /// Represents the type of contact information.
    /// </summary>
    public enum ContactInformationType
    {
        /// <summary>
        /// Email contact information.
        /// </summary>
        Email,

        /// <summary>
        /// Phone number contact information.
        /// </summary>
        PhoneNumber,

        /// <summary>
        /// Postal address contact information.
        /// </summary>
        PostalAddress,

        /// <summary>
        /// Visiting address contact information.
        /// </summary>
        VisitingAddress
    };

    /// <summary>
    /// Represents the language codes for descriptive items.
    /// </summary>
    public enum DescriptiveItemLanguageCode { 
        /// <summary>
        /// English language code.
        /// </summary>
        En, 
        /// <summary>
        /// Finnish language code.
        /// </summary>
        Fi, 
        /// <summary>
        /// Swedish language code.
        /// </summary>
        Sv 
    };

    /// <summary>
    /// Represents the PID type of an organization.
    /// </summary>
    public enum OrganizationPidType
    {
        /// <summary>
        /// Business ID.
        /// </summary>
        BusinessId
    };

    /// <summary>
    /// Represents the validity of a relationship.
    /// </summary>
    public enum ValidRelationship
    {
        /// <summary>
        /// The relationship is valid.
        /// </summary>
        The0,

        /// <summary>
        /// The relationship is invalid.
        /// </summary>
        The1
    };
}
