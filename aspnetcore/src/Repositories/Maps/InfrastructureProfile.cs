using AutoMapper;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models.Infrastructure;
using Infrastructure = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;

namespace CSC.PublicApi.Repositories.Maps;

public class InfrastructureProfile : Profile
{
    private const string DimPid_PidType_Urn = "URN";
    private const string DescriptiveItemType_Name = "name";
    private const string DescriptiveItemType_Description = "description";
    private const string DescriptiveItemType_ScientificDescription = "scientific_description";
    private const string WebLinkType_Homepage = "homepage";
    private const string WebLinkType_TermsOfUse = "terms_of_use";
    private const string ContactInformationType_Email = "email";
    private const string ContactInformationType_PhoneNumber = "phone_number";
    private const string ContactInformationType_PostalAddress = "postal_address";
    private const string ContactInformationType_VisitingAddress = "visiting_address";

    public InfrastructureProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateProjection<DimInfrastructure, Infrastructure>()
            .ForMember(dst => dst.ExportSortId, opt => opt.MapFrom(src => (long)src.Id))
            // URN
            .ForMember(dst => dst.InfrastructureUrn, opt => opt.MapFrom(src => src.DimPids.Where(dp => dp.PidType == DimPid_PidType_Urn)
                .Select(dp => dp.PidContent).FirstOrDefault()))
            // Acronym
            .ForMember(dst => dst.Abbreviation, opt => opt.MapFrom(src => src.Acronym))
            // Starting year
            .ForMember(dst => dst.StartingYear, opt => opt.MapFrom(src => src.StartingYear))
            // End year - TODO when new field is added in database
            // On the academy of Finland road map
            .ForMember(dst => dst.FinlandRoadmap, opt => opt.MapFrom(src => src.FinlandRoadmap))
            // Name
            .ForMember(dst => dst.InfraName, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_Name)))
            // Description
            .ForMember(dst => dst.InfraDescription, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_Description)))
            // Scientific description
            .ForMember(dst => dst.InfraScientificDescription, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_ScientificDescription)))
            // ESFRI code
            .ForMember(dst => dst.EsfriCode, opt => opt.MapFrom(src => src.DimReferencedata))
            // Researchfi landing page (hardcoded)
            .ForMember(dst => dst.ResearchFiLandingPage, opt => opt.MapFrom(src => "TODO"))
            // Keywords
            .ForMember(dst => dst.Keywords, opt => opt.MapFrom(src => src.FactInfraKeywords
                .Select(k => k.DimKeyword)))
            // Web link - Homepage
            .ForMember(dst => dst.InfraHomepage, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Homepage)))
            // Web link - Terms of use
            .ForMember(dst => dst.InfraTermsOfUse, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_TermsOfUse)))
            // Service
            .ForMember(dst => dst.InfrastructuresService, opt => opt.MapFrom(src => src.DimServices))
            // Contact information
            .ForMember(dst => dst.DimInfrastructureId1, opt => opt.MapFrom(src => src.DimContactInformations));

        CreateProjection<DimReferencedatum, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimDescriptiveItem, DescriptiveItemStruct>()
            .ForMember(dst => dst.DescriptiveItem, opt => opt.MapFrom(src => src.DescriptiveItem))
            .ForMember(dst => dst.DescriptiveItemLanguage, opt => opt.MapFrom(src => src.DescriptiveItemLanguage))
            .ForMember(dst => dst.DescriptiveItemTypeName, opt => opt.MapFrom(src => src.DescriptiveItemType));

        CreateProjection<DimKeyword, Keywords>()
            .ForMember(dst => dst.DimKeywordLanguage, opt => opt.MapFrom(src => src.Language))
            .ForMember(dst => dst.Keyword, opt => opt.MapFrom(src => src.Keyword));

        CreateProjection<DimWebLink, Weblink>()
            .ForMember(dst => dst.LanguageVariant, opt => opt.MapFrom(src => src.LanguageVariant))
            .ForMember(dst => dst.LinkLabel, opt => opt.MapFrom(src => src.LinkLabel))
            .ForMember(dst => dst.WebLinkUrl, opt => opt.MapFrom(src => src.Url));

        CreateProjection<DimService, ResearchInfrastructureService>()
            .ForMember(dst => dst.DimServiceId1, opt => opt.MapFrom(src => src.DimContactInformations))
            //.ForMember(dst => dst.OtherIdentifierService, opt => opt.MapFrom(src => src.))
            //.ForMember(dst => dst.ServiceUrn, opt => opt.MapFrom(src => src.))
            .ForMember(dst => dst.ServiceName, opt => opt.MapFrom(
                src => src.DimDescriptiveItems.Where(di => di.DescriptiveItemType == DescriptiveItemType_Name)))
            .ForMember(dst => dst.ServiceDescription, opt => opt.MapFrom(
                src => src.DimDescriptiveItems.Where(di => di.DescriptiveItemType == DescriptiveItemType_Description)))
            .ForMember(dst => dst.ServiceWebsite, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Homepage)));

        CreateProjection<DimContactInformation, ContactInformation>()
            .ForMember(dst => dst.ContactInformationContent, opt => opt.MapFrom(src => src.ContactInformationContent))
            .ForMember(dst => dst.ContactInformationType, opt => opt.MapFrom(src => src.ContactInformationType))
            .ForMember(dst => dst.ContactName, opt => opt.MapFrom(src => src.ContactName));
    }
}
