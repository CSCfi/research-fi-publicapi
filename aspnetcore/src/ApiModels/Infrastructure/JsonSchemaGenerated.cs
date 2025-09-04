namespace Generated
{
    using System.Collections.Generic;

    /// <summary></summary>
    public partial class ResearchInfrastructure
    {
        /// <summary></summary>
        public DescriptiveItemDescription? ClDescription { get; set; }
        /// <summary></summary>
        public DescriptiveItemName? ClName { get; set; }
        /// <summary></summary>
        public ServiceOtherIdentifier? ClPidTypesService { get; set; }
        /// <summary></summary>
        public DescriptiveItemScientificDesription? ClScientificDescription { get; set; }
        /// <summary></summary>
        public ContactInformation? DimContactInformation { get; set; }
        /// <summary></summary>
        public DimInfrastructureClass? DimInfrastructure { get; set; }
        /// <summary></summary>
        public Keywords? DimKeyword { get; set; }
        /// <summary></summary>
        public ResearchInfrastructureService? DimService { get; set; }
        /// <summary></summary>
        public Weblink? DimWebLink { get; set; }
        /// <summary></summary>
        public OrganizationInformation? OrganizationInformation { get; set; }
        /// <summary></summary>
        public InfrastructureIdentifiers? PidTypesInfra { get; set; }
        /// <summary></summary>
        public InfrastructureRelations? RelationshipToInfrastructure { get; set; }
    }

    /// <summary></summary>
    public partial class DescriptiveItemDescription
    {
        /// <summary></summary>
        public string? DescriptiveItem { get; set; }
        /// <summary></summary>
        public DescriptiveItemLanguageCode DescriptiveItemLanguage { get; set; }
        /// <summary></summary>
        public DescriptiveItemTypeDescription DescriptiveItemTypeDescription { get; set; }
    }

    /// <summary></summary>
    public partial class DescriptiveItemName
    {
        /// <summary></summary>
        public string? DescriptiveItem { get; set; }
        /// <summary></summary>
        public DescriptiveItemLanguageCode? DescriptiveItemLanguage { get; set; }
        /// <summary></summary>
        public DescriptiveItemTypeName? DescriptiveItemTypeName { get; set; }
    }

    /// <summary></summary>
    public partial class ServiceOtherIdentifier
    {
        /// <summary></summary>
        public string? PidContentService { get; set; }
        /// <summary></summary>
        public EIdentifierType? PidTypeService { get; set; }
    }

    /// <summary></summary>
    public partial class DescriptiveItemScientificDesription
    {
        /// <summary></summary>
        public string? DescriptiveItem { get; set; }
        /// <summary></summary>
        public DescriptiveItemLanguageCode? DescriptiveItemLanguage { get; set; }
        /// <summary></summary>
        public DescriptiveItemTypeScientificDescription? DescriptiveItemTypeScientificDesription { get; set; }
    }

    /// <summary></summary>
    public partial class ContactInformation
    {
        /// <summary></summary>
        public string? ContactInformationContent { get; set; }
        /// <summary></summary>
        public ContactInformationType? ContactInformationType { get; set; }
        /// <summary></summary>
        public string? ContactName { get; set; }
    }

    /// <summary></summary>
    public partial class DimInfrastructureClass
    {
        /// <summary></summary>
        public string? Acronym { get; set; }
        /// <summary></summary>
        public List<ContactInformation>? DimInfrastructureId1 { get; set; }
        /// <summary></summary>
        public List<string>? EsfriCode { get; set; }
        /// <summary></summary>
        public List<string>? FieldOfScience { get; set; }
        /// <summary></summary>
        public bool FinlandRoadmap { get; set; }

        /// <summary></summary>
        public List<DescriptiveItemDescription>? InfraDescription { get; set; }

        /// <summary></summary>
        public List<Weblink>? InfraHomepage { get; set; }

        /// <summary></summary>
        public List<DescriptiveItemName>? InfraName { get; set; }

        /// <summary></summary>
        public List<DescriptiveItemScientificDesription>? InfraScientificDescription { get; set; }

        /// <summary>Infrastructure URN, key identifier in Research.fi</summary>
        public string? InfrastructureUrn { get; set; }

        /// <summary></summary>
        public List<ResearchInfrastructureService>? InfrastructuresService { get; set; }
        /// <summary></summary>
        public List<Weblink>? InfraTermsOfUse { get; set; }
        /// <summary></summary>
        public List<Keywords>? Keywords { get; set; }
        /// <summary></summary>
        public List<OrganizationInformation>? OrganizationRelatedToInfra { get; set; }
        /// <summary></summary>
        public List<InfrastructureIdentifiers>? OtherIdentifierInfra { get; set; }
        /// <summary></summary>
        public List<InfrastructureRelations>? RelatedInfrastructure { get; set; }

        /// <summary>https://tiedejatutkimus.fi/en/results/infrastructure/ + Infrastructure key identifier; [URN]</summary>
        public string? ResearchFiLandingPage { get; set; }
        /// <summary></summary>
        public OrganizationInformation? ResponsibleOrganization { get; set; }
        /// <summary></summary>
        public double StartingYear { get; set; }
    }

    /// <summary></summary>
    public partial class Weblink
    {
        /// <summary></summary>
        public string? LanguageVariant { get; set; }
        /// <summary></summary>
        public string? LinkLabel { get; set; }
        /// <summary></summary>
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
        public List<DescriptiveItemDescription>? ServiceDescription { get; set; }

        /// <summary>fi / en / sv</summary>
        public List<DescriptiveItemName>? ServiceName { get; set; }
        /// <summary></summary>
        public List<Weblink>? ServiceWebsite { get; set; }
    }

    /// <summary></summary>
    public partial class Keywords
    {
        /// <summary></summary>
        public string? DimKeywordLanguage { get; set; }
        /// <summary></summary>
        public string? Keyword { get; set; }
    }

    /// <summary></summary>
    public partial class OrganizationInformation
    {
        /// <summary></summary>
        public string? OrganizationIdentifier { get; set; }
        /// <summary></summary>
        public OrganizationPidType OrganizationPidType { get; set; }
    }

    /// <summary></summary>
    public partial class InfrastructureIdentifiers
    {
        /// <summary></summary>
        public string? PidContentInfra { get; set; }

        /// <summary>Organisaation paikallinen tunniste infrastruktuurille</summary>
        public EIdentifierType PidTypeInfra { get; set; }
    }

    /// <summary></summary>
    public partial class InfrastructureRelations
    {
        /// <summary></summary>
        public string? InternationalInfraName { get; set; }
        /// <summary></summary>
        public string? InternationalInfraWeblink { get; set; }
        /// <summary></summary>
        public List<InfrastructureIdentifiers>? OtherIdentifierRelatedInfra { get; set; }
        /// <summary></summary>
        public string? RelatedInfraPid { get; set; }
        /// <summary></summary>
        public string? RelationEndDate { get; set; }
        /// <summary></summary>
        public string? RelationTypeCode { get; set; }
        /// <summary></summary>
        public string? RelationshipStartDate { get; set; }
        /// <summary></summary>
        public ValidRelationship ValidRelation { get; set; }
    }

    /// <summary></summary>
    public enum DescriptiveItemLanguageCode {
        /// <summary></summary>
        En,
        /// <summary></summary>
        Fi,
        /// <summary></summary>
        Sv
    };

    /// <summary></summary>
    public enum DescriptiveItemTypeDescription {
        /// <summary></summary>
        Description
    };

    /// <summary></summary>
    public enum DescriptiveItemTypeName {
        /// <summary></summary>
        Name
    };

    /// <summary>Organisaation paikallinen tunniste infrastruktuurille</summary>
    public enum EIdentifierType {
        /// <summary></summary>
        LocalIdentifier
    };

    /// <summary></summary>
    public enum DescriptiveItemTypeScientificDescription {
        /// <summary></summary>
        ScientificDescription
    };

    /// <summary></summary>
    public enum ContactInformationType {
        /// <summary></summary>
        Email,
        /// <summary></summary>
        PhoneNumber,
        /// <summary></summary>
        PostalAddress,
        /// <summary></summary>
        VisitingAddress
    };

    /// <summary></summary>
    public enum OrganizationPidType {
        /// <summary></summary>
        BusinessId
    };

    /// <summary></summary>
    public enum ValidRelationship {
        /// <summary></summary>
        The0,
        /// <summary></summary>
        The1
    };
}
