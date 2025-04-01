using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingCall;
using FundingCall = CSC.PublicApi.Service.Models.FundingCall.FundingCall;

namespace CSC.PublicApi.Repositories.Maps;

public class FundingCallProfile : Profile
{
    public FundingCallProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateProjection<DimCallProgramme, FundingCall>()
            .ForMember(dst => dst.ExportSortId, opt => opt.MapFrom(src => (long)src.Id))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
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
            .ForMember(dst => dst.Foundations, opt => opt.MapFrom(src => src.DimOrganizations))
            .ForMember(dst => dst.ContinuousApplication, opt => opt.MapFrom(src => src.DimDateIdOpen == -1 && src.DimDateIdDue == -1)) // src.ContinuousApplicationPeriod is not populated in DB!
            .ForMember(dst => dst.ApplicationUrlFi, opt => opt.MapFrom(src => src.DimWebLinks.SingleOrDefault(webLink => webLink.LinkType == "ApplicationURL" && webLink.LanguageVariant == "fi")))
            .ForMember(dst => dst.ApplicationUrlSv, opt => opt.MapFrom(src => src.DimWebLinks.SingleOrDefault(webLink => webLink.LinkType == "ApplicationURL" && webLink.LanguageVariant == "sv")))
            .ForMember(dst => dst.ApplicationUrlEn, opt => opt.MapFrom(src => src.DimWebLinks.SingleOrDefault(webLink => webLink.LinkType == "ApplicationURL" && webLink.LanguageVariant == "en")))
            .ForMember(dst => dst.SourceId, opt => opt.MapFrom(src => src.SourceId))
            .ForMember(dst => dst.SourceDescription, opt => opt.MapFrom(src => src.SourceDescription))
            .ForMember(dst => dst.SourceProgrammeId, opt => opt.MapFrom(src => src.SourceProgrammeId))
            .ForMember(dst => dst.Abbreviation, opt => opt.MapFrom(src => src.Abbreviation))
            .ForMember(dst => dst.EuCallId, opt => opt.MapFrom(src => src.EuCallId))
            .ForMember(dst => dst.ResearchfiUrl, opt => opt.Ignore()); // Handled during in memory operations in the index repository

        CreateProjection<DimReferencedatum, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimOrganization, Foundation>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
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