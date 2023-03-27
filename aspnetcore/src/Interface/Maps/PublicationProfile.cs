using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Publication;

namespace CSC.PublicApi.Interface.Maps;

public class PublicationProfile : Profile
{
    private const string DateTimeYearFormat = "yyyy";

    public PublicationProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateMap<GetPublicationsQueryParameters, PublicationSearchParameters>()
            .ForMember(dst => dst.TypeCode, opt => opt.MapFrom(src =>  src.TypeCode.ToLower()));
        
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
        CreateMap<Service.Models.ReferenceData, ReferenceData>();
        CreateMap<Service.Models.Keyword, Keyword>();
    }
}