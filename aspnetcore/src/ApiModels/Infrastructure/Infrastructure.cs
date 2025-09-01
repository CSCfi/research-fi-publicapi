namespace ResearchFi.Infrastructure
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a coordinate with various descriptive and relational properties.
    /// </summary>
    public partial class Coordinate
    {
        /// <summary>
        /// Description of the coordinate.
        /// </summary>
        public DescriptiveItemDescription? ClDescription { get; set; }

        /// <summary>
        /// Name of the coordinate.
        /// </summary>
        public DescriptiveItemName? ClName { get; set; }

        /// <summary>
        /// Service identifier for the coordinate.
        /// </summary>
        public ServiceOtherIdentifier? ClPidTypesService { get; set; }

        /// <summary>
        /// Scientific description of the coordinate.
        /// </summary>
        public DescriptiveItemScientificDesription? ClScientificDescription { get; set; }

        /// <summary>
        /// Contact information for the coordinate.
        /// </summary>
        public ContactInformation? DimContactInformation { get; set; }

        /// <summary>
        /// Infrastructure details.
        /// </summary>
        public Infrastructure? DimInfrastructure { get; set; }

        /// <summary>
        /// Keywords associated with the coordinate.
        /// </summary>
        public Keywords? DimKeyword { get; set; }

        /// <summary>
        /// Service details for the coordinate.
        /// </summary>
        public ResearchInfrastructureService? DimService { get; set; }

        /// <summary>
        /// Web link details.
        /// </summary>
        public Weblink? DimWebLink { get; set; }

        /// <summary>
        /// Organization information.
        /// </summary>
        public OrganizationInformation? OrganizationInformation { get; set; }

        /// <summary>
        /// Infrastructure identifiers.
        /// </summary>
        public InfrastructureIdentifiers? PidTypesInfra { get; set; }

        /// <summary>
        /// Relationship to other infrastructures.
        /// </summary>
        public InfrastructureRelations? RelationshipToInfrastructure { get; set; }
    }

    /// <summary>
    /// Represents a descriptive item description with language and type information.
    /// </summary>
    /// <summary>fi / en / sv</summary>
    public partial class DescriptiveItemDescription
    {
        /// <summary>
        /// Descriptive item content.
        /// </summary>
        public string? DescriptiveItem { get; set; }

        /// <summary>
        /// Language code for the descriptive item.
        /// </summary>
        public string? DescriptiveItemLanguage { get; set; }

        /// <summary>
        /// Type description of the descriptive item.
        /// </summary>
        public DescriptiveItemTypeDescription? DescriptiveItemTypeDescription { get; set; }
    }

    /// <summary>
    /// Represents a descriptive item name with language and type information.
    /// </summary>
    /// <summary>fi / en / sv</summary>
    public partial class DescriptiveItemName
    {
        /// <summary>
        /// Name of the descriptive item.
        /// </summary>
        public string? DescriptiveItem { get; set; }

        /// <summary>
        /// Language code for the descriptive item name.
        /// </summary>
        public string? DescriptiveItemLanguage { get; set; }

        /// <summary>
        /// Type name of the descriptive item.
        /// </summary>
        public string? DescriptiveItemTypeName { get; set; }
    }

    /// <summary>
    /// Represents a service identifier with content and type.
    /// </summary>
    public partial class ServiceOtherIdentifier
    {
        /// <summary>
        /// Content of the service identifier.
        /// </summary>
        public string? PidContentService { get; set; }

        /// <summary>
        /// Type of the service identifier.
        /// </summary>
        public EIdentifierType? PidTypeService { get; set; }
    }

    /// <summary>
    /// Represents a scientific description with language and type information.
    /// </summary>
    /// <summary>fi / en / sv</summary>
    public partial class DescriptiveItemScientificDesription
    {
        /// <summary>
        /// Scientific description content.
        /// </summary>
        public string? DescriptiveItem { get; set; }

        /// <summary>
        /// Language code for the scientific description.
        /// </summary>
        public string? DescriptiveItemLanguage { get; set; }

        /// <summary>
        /// Type of the scientific description.
        /// </summary>
        public DescriptiveItemTypeScientificDescription? DescriptiveItemTypeScientificDesription { get; set; }
    }

    /// <summary>
    /// Represents contact information with content, type, and name.
    /// </summary>
    public partial class ContactInformation
    {
        /// <summary>
        /// Content of the contact information.
        /// </summary>
        public string? ContactInformationContent { get; set; }

        /// <summary>
        /// Type of the contact information.
        /// </summary>
        public ContactInformationType? ContactInformationType { get; set; }

        /// <summary>
        /// Name of the contact person.
        /// </summary>
        public string? ContactName { get; set; }
    }

    /// <summary>
    /// Represents a research infrastructure with various descriptive and relational properties.
    /// </summary>
    public partial class Infrastructure
    {
        /// <summary>
        /// Acronym of the research infrastructure.
        /// </summary>
        public string? Acronym { get; set; }

        /// <summary>
        /// Contact information for the infrastructure.
        /// </summary>
        public List<ContactInformation>? DimInfrastructureId1 { get; set; }

        /// <summary>
        /// ESFRI code of the infrastructure.
        /// </summary>
        public List<string>? EsfriCode { get; set; }

        /// <summary>
        /// Fields of science related to the infrastructure.
        /// </summary>
        public List<string>? FieldOfScience { get; set; }

        /// <summary>
        /// Indicates if the infrastructure is part of Finland's roadmap.
        /// </summary>
        public bool? FinlandRoadmap { get; set; }

        /// <summary>
        /// Descriptions of the infrastructure.
        /// </summary>
        public List<DescriptiveItemDescription>? InfraDescription { get; set; }

        /// <summary>
        /// Homepage links for the infrastructure.
        /// </summary>
        public List<Weblink>? InfraHomepage { get; set; }

        /// <summary>
        /// Names of the infrastructure.
        /// </summary>
        public List<DescriptiveItemName>? InfraName { get; set; }

        /// <summary>
        /// Scientific descriptions of the infrastructure.
        /// </summary>
        public List<DescriptiveItemScientificDesription>? InfraScientificDescription { get; set; }

        /// <summary>
        /// URN of the infrastructure.
        /// </summary>
        public string? InfrastructureUrn { get; set; }

        /// <summary>
        /// Services provided by the infrastructure.
        /// </summary>
        public ResearchInfrastructureService? InfrastructuresService { get; set; }

        /// <summary>
        /// Terms of use for the infrastructure.
        /// </summary>
        public List<Weblink>? InfraTermsOfUse { get; set; }

        /// <summary>
        /// Keywords associated with the infrastructure.
        /// </summary>
        public List<Keywords>? Keywords { get; set; }

        /// <summary>
        /// Organizations related to the infrastructure.
        /// </summary>
        public List<OrganizationInformation>? OrganizationRelatedToInfra { get; set; }

        /// <summary>
        /// Other identifiers for the infrastructure.
        /// </summary>
        public List<InfrastructureIdentifiers>? OtherIdentifierInfra { get; set; }

        /// <summary>
        /// Related infrastructures.
        /// </summary>
        public List<InfrastructureRelations>? RelatedInfrastructure { get; set; }

        /// <summary>
        /// Landing page for the infrastructure on Research.fi.
        /// </summary>
        public string? ResearchFiLandingPage { get; set; }

        /// <summary>
        /// Responsible organization for the infrastructure.
        /// </summary>
        public OrganizationInformation? ResponsibleOrganization { get; set; }

        /// <summary>
        /// Starting year of the infrastructure.
        /// </summary>
        public double? StartingYear { get; set; }
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

    /// <summary>
    /// Represents a research infrastructure service with various descriptive and relational properties.
    /// </summary>
    public partial class ResearchInfrastructureService
    {
        /// <summary>
        /// Contact information for the service.
        /// </summary>
        public List<ContactInformation>? DimServiceId1 { get; set; }

        /// <summary>
        /// Other identifiers for the service.
        /// </summary>
        public List<ServiceOtherIdentifier>? OtherIdentifierService { get; set; }

        /// <summary>
        /// Websites for the service.
        /// </summary>
        public List<Weblink>? ServiceWebsite { get; set; }
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
    /// Represents organization information with identifier and PID type.
    /// </summary>
    public partial class OrganizationInformation
    {
        /// <summary>
        /// Identifier for the organization.
        /// </summary>
        public string? OrganizationIdentifier { get; set; }

        /// <summary>
        /// PID type for the organization.
        /// </summary>
        public OrganizationPidType? OrganizationPidType { get; set; }
    }

    /// <summary>
    /// Represents infrastructure identifiers with content and type.
    /// </summary>
    public partial class InfrastructureIdentifiers
    {
        /// <summary>
        /// Content of the infrastructure identifier.
        /// </summary>
        public string? PidContentInfra { get; set; }
        /// <summary>Organisaation paikallinen tunniste infrastruktuurille</summary>
        public EIdentifierType PidTypeInfra { get; set; }
    }

    /// <summary>
    /// Represents relationships between infrastructures with various descriptive properties.
    /// </summary>
    public partial class InfrastructureRelations
    {
        /// <summary>
        /// Name of the related international infrastructure.
        /// </summary>
        public string? InternationalInfraName { get; set; }

        /// <summary>
        /// Web link to the related international infrastructure.
        /// </summary>
        public string? InternationalInfraWeblink { get; set; }

        /// <summary>
        /// Other identifiers for the related infrastructure.
        /// </summary>
        public List<InfrastructureIdentifiers>? OtherIdentifierRelatedInfra { get; set; }

        /// <summary>
        /// PID of the related infrastructure.
        /// </summary>
        public string? RelatedInfraPid { get; set; }

        /// <summary>
        /// End date of the relationship.
        /// </summary>
        public string? RelationEndDate { get; set; }

        /// <summary>
        /// Type code of the relationship.
        /// </summary>
        public string? RelationTypeCode { get; set; }

        /// <summary>
        /// Start date of the relationship.
        /// </summary>
        public string? RelationshipStartDate { get; set; }

        /// <summary>
        /// Validity of the relationship.
        /// </summary>
        public ValidRelationship? ValidRelation { get; set; }
    }

    /// <summary>
    /// Types of descriptive item descriptions.
    /// </summary>
    public enum DescriptiveItemTypeDescription
    {
        /// <summary>
        /// General description.
        /// </summary>
        Description
    };

    /// <summary>
    /// Types of descriptive item names.
    /// </summary>
    public enum DescriptiveItemTypeName
    {
        /// <summary>
        /// General name.
        /// </summary>
        Name
    };

    /// <summary>
    /// Types of infrastructure identifiers.
    /// </summary>
    public enum EIdentifierType
    {
        /// <summary>
        /// Local identifier.
        /// </summary>
        LocalIdentifier
    };

    /// <summary>
    /// Types of scientific descriptions.
    /// </summary>
    public enum DescriptiveItemTypeScientificDescription
    {
        /// <summary>
        /// Scientific description.
        /// </summary>
        ScientificDescription
    };

    /// <summary>
    /// Types of contact information.
    /// </summary>
    public enum ContactInformationType
    {
        /// <summary>
        /// Email contact type.
        /// </summary>
        Email,

        /// <summary>
        /// Phone number contact type.
        /// </summary>
        PhoneNumber,

        /// <summary>
        /// Postal address contact type.
        /// </summary>
        PostalAddress,

        /// <summary>
        /// Visiting address contact type.
        /// </summary>
        VisitingAddress
    };

    /// <summary>
    /// Types of organization PID.
    /// </summary>
    public enum OrganizationPidType
    {
        /// <summary>
        /// Business ID type.
        /// </summary>
        BusinessId
    };

    /// <summary>
    /// Validity of relationships.
    /// </summary>
    public enum ValidRelationship
    {
        /// <summary>
        /// Valid relationship type 0.
        /// </summary>
        The0,

        /// <summary>
        /// Valid relationship type 1.
        /// </summary>
        The1
    };
}
