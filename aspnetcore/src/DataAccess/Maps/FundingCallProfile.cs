using AutoMapper;
using CSC.PublicApi.DataAccess.Entities;
using CSC.PublicApi.Service.Models.FundingCall;
using FundingCall = CSC.PublicApi.Service.Models.FundingCall.FundingCall;

namespace CSC.PublicApi.DataAccess.Maps;

public class FundingCallProfile : Profile
{
    public FundingCallProfile()
    {
        CreateProjection<DimCallProgramme, FundingCall>()
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.DescriptionFi, opt => opt.MapFrom(src => src.DescriptionFi))
            .ForMember(dst => dst.DescriptionSv, opt => opt.MapFrom(src => src.DescriptionSv))
            .ForMember(dst => dst.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
            .ForMember(dst => dst.ApplicationTermsFi, opt => opt.MapFrom(src => src.ApplicationTermsFi))
            .ForMember(dst => dst.ApplicationTermsSv, opt => opt.MapFrom(src => src.ApplicationTermsSv))
            .ForMember(dst => dst.ApplicationTermsEn, opt => opt.MapFrom(src => src.ApplicationTermsEn))
            .ForMember(dst => dst.CallProgrammeOpenDate, opt => opt.MapFrom(src => src.DimDateIdOpenNavigation))
            .ForMember(dst => dst.CallProgrammeDueDate, opt => opt.MapFrom(src => src.DimDateIdDueNavigation))
            .ForMember(dst => dst.ContactInformation, opt => opt.MapFrom(src => src.ContactInformation))
            .ForMember(dst => dst.Categories, opt => opt.MapFrom(src => src.DimReferencedata))
            .ForMember(dst => dst.Foundation, opt => opt.MapFrom(src => src.DimOrganizations))
            .ForMember(dst => dst.ContinuosApplication, opt => opt.MapFrom(src => src.ContinuousApplicationPeriod))
            .ForMember(dst => dst.ApplicationURLFi, opt => opt.MapFrom(src => src.DimWebLinks.SingleOrDefault(webLink => webLink.LinkType == "ApplicationURL" && webLink.LanguageVariant == "fi")))
            .ForMember(dst => dst.ApplicationURLSv, opt => opt.MapFrom(src => src.DimWebLinks.SingleOrDefault(webLink => webLink.LinkType == "ApplicationURL" && webLink.LanguageVariant == "sv")))
            .ForMember(dst => dst.ApplicationURLEn, opt => opt.MapFrom(src => src.DimWebLinks.SingleOrDefault(webLink => webLink.LinkType == "ApplicationURL" && webLink.LanguageVariant == "en")));

        CreateProjection<DimReferencedatum, Category>()
            .ForMember(dst => dst.CodeValue, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimOrganization, Foundation>()
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.BusinessId, opt => opt.MapFrom(src => src.OrganizationId))
            .ForMember(dst => dst.FoundationUrl, opt => opt.MapFrom(src => src.DimWebLinks.SingleOrDefault(webLink => webLink.LinkType == "FoundationURL")));

        CreateProjection<DimDate, DateTime?>()
            .ConvertUsing(x => x.Id != -1 ? new DateTime(x.Year, x.Month, x.Day) : null);

        CreateProjection<DimWebLink, string?>()
            .ConvertUsing(x => x != null ? x.Url : null);

    }
}