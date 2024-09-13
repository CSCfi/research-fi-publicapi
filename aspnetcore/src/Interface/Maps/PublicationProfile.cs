using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi;
using ResearchFi.CodeList;
using ResearchFi.Publication;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps;

public class PublicationProfile : Profile
{
    private const string DateTimeYearFormat = "yyyy";

    public PublicationProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateMap<GetPublicationsQueryParameters, PublicationSearchParameters>()
            .ForMember(dst => dst.TypeCode, opt => opt.MapFrom(src =>  src.Type!));
        
        CreateMap<Service.Models.Publication.Publication, Publication>()
            .ForMember(dst => dst.PublicationYear, opt => opt.MapFrom(src =>  src.PublicationYear.HasValue ? src.PublicationYear.Value.ToString(DateTimeYearFormat) : null))
            .ForMember(dst => dst.ReportingYear, opt => opt.MapFrom(src =>  src.ReportingYear.HasValue ? src.ReportingYear.Value.ToString(DateTimeYearFormat) : null))
            .ForMember(dst => dst.ApcPaymentYear, opt => opt.MapFrom(src =>  src.ApcPaymentYear.HasValue ? src.ApcPaymentYear.Value.ToString(DateTimeYearFormat) : null));

        CreateMap<Service.Models.Publication.Organization, Organization>();
        CreateMap<Service.Models.Publication.OrganizationUnit, OrganizationUnit>();
        CreateMap<Service.Models.Publication.ParentPublication, ParentPublication>();
        CreateMap<Service.Models.Publication.Author, Author>();
        CreateMap<Service.Models.Publication.AuthorOrganization, AuthorOrganization>();
        CreateMap<Service.Models.Publication.LocallyReportedPublicationInformation, LocallyReportedPublicationInformation>();
        CreateMap<Service.Models.ReferenceData, ArtPublicationRole>();
        CreateMap<Service.Models.ReferenceData, PublicationFormat>();
        CreateMap<Service.Models.ReferenceData, PeerReviewed>();
        CreateMap<Service.Models.ReferenceData, TargetAudience>();
        CreateMap<Service.Models.ReferenceData, PublicationType>();
        CreateMap<Service.Models.ReferenceData, ParentPublicationType>();
        CreateMap<Service.Models.ReferenceData, JufoClass>();
        CreateMap<Service.Models.ReferenceData, FieldOfScience>();
        CreateMap<Service.Models.ReferenceData, FieldOfEducation>();
        CreateMap<Service.Models.ReferenceData, FieldOfArt>();
        CreateMap<Service.Models.ReferenceData, ArticleType>();
        CreateMap<Service.Models.ReferenceData, ArtPublicationTypeCategory>();
        CreateMap<Service.Models.ReferenceData, PublicationStatus>();
        CreateMap<Service.Models.ReferenceData, License>();
        CreateMap<Service.Models.ReferenceData, CountryCode>();
        CreateMap<Service.Models.ReferenceData, Language>();
        CreateMap<Service.Models.ReferenceData, SelfArchiveVersion>();
        CreateMap<Service.Models.ReferenceData, OpenAccess>();
        CreateMap<Service.Models.ReferenceData, PublisherOpenAccess>();
        CreateMap<Service.Models.ReferenceData, ThesisType>();
        CreateMap<Service.Models.Keyword, Keyword>();
    }
}