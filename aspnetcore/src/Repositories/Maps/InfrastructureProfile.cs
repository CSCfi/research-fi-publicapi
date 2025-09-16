using AutoMapper;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models.Infrastructure;
using Infrastructure = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;
using System.Collections.Immutable;

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
    private const string DimReferencedata_CodeScheme_FieldOfScience = "Tieteenala2010";

    public InfrastructureProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateProjection<DimInfrastructure, Infrastructure>()
            .ForMember(dst => dst.ExportSortId, opt => opt.MapFrom(src => (long)src.Id))
            // URN
            .ForMember(dst => dst.InfraIdentifier, opt => opt.MapFrom(src =>
                src.DimPids.FirstOrDefault() == null ? null : new Identifier
                {
                    PersistentIdentifierUrn = src.DimPids.Where(dp => dp.PidType == DimPid_PidType_Urn).Select(dp => dp.PidContent).FirstOrDefault(),
                    OtherPid = src.DimPids.Where(dp => dp.PidType != DimPid_PidType_Urn).Select(dp => new OtherPersistentIdentifier
                    {
                        Pid = dp.PidContent,
                        PidType = dp.PidType
                    }).ToList()
                    // TODO: InfraLocalIdentifier
                }))
            // Acronym
            .ForMember(dst => dst.Acronym, opt => opt.MapFrom(src => src.Acronym))
            // Starting year
            .ForMember(dst => dst.StartingYear, opt => opt.MapFrom(src => src.StartingYear))
            // TODO: End year to be added when field is available in the DB
            // On the academy of Finland road map
            .ForMember(dst => dst.FinlandRoadmapInfrastructure, opt => opt.MapFrom(src => src.FinlandRoadmap))
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
            .ForMember(dst => dst.Esfri, opt => opt.MapFrom(src => src.DimReferencedata))
            // Researchfi landing page (hardcoded)
            .ForMember(dst => dst.ResearchFiLandingPage, opt => opt.MapFrom(src => "TODO"))
            // Keywords
            .ForMember(dst => dst.InfraKeyword, opt => opt.MapFrom(src => src.FactInfraKeywords
                .Select(k => k.DimKeyword)))
            // Web link - Homepage
            .ForMember(dst => dst.InfraHomepage, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Homepage)))
            // Web link - Terms of use
            .ForMember(dst => dst.TermsOfUse, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_TermsOfUse).FirstOrDefault()))
            // Service
            .ForMember(dst => dst.Service, opt => opt.MapFrom(src => src.DimServices.FirstOrDefault()))
            // Contact information
            .ForMember(dst => dst.InfraContactInformation, opt => opt.MapFrom(src => src.DimContactInformations))
            // Infrastructure network
            // TODO: .ForMember(dst => dst.Relation, opt => opt.MapFrom(src => src.fact))
            // Field of science
            .ForMember(dst => dst.FieldOfScience, opt => opt.MapFrom(src => src.FactDimReferencedataFieldOfSciences.ToList().Select(f => f.DimReferencedata)));

        CreateProjection<DimReferencedatum, CSC.PublicApi.Service.Models.Infrastructure.ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.CodeValue, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.CodeDescriptionFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.CodeDescriptionSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.CodeDescriptionEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimDescriptiveItem, DescriptiveTerm>()
            .ForMember(dst => dst.DescriptiveTermDescriptiveTerm, opt => opt.MapFrom(src => src.DescriptiveItem))
            .ForMember(dst => dst.LanguageCodeKeyword, opt => opt.MapFrom(src => src.DescriptiveItemLanguage));

        CreateProjection<DimDescriptiveItem, ContentDescriptionText>()
            .ForMember(dst => dst.Desciptive, opt => opt.MapFrom(src => src.DescriptiveItem))
            .ForMember(dst => dst.LanguageCodeVariant, opt => opt.MapFrom(src => ConvertToLanguageCode(src.DescriptiveItemLanguage)));

        CreateProjection<DimKeyword, DescriptiveTerm>()
            .ForMember(dst => dst.LanguageCodeKeyword, opt => opt.MapFrom(src => src.Language))
            .ForMember(dst => dst.DescriptiveTermDescriptiveTerm, opt => opt.MapFrom(src => src.Keyword));

        CreateProjection<DimWebLink, Weblink>()
            .ForMember(dst => dst.LanguageCodeWeblink, opt => opt.MapFrom(src => src.LanguageVariant))
            .ForMember(dst => dst.WeblinkLabel, opt => opt.MapFrom(src => src.LinkLabel))
            .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.Url));

        CreateProjection<DimService, InfrastructureSService>()
            .ForMember(dst => dst.ServiceContactInformation, opt => opt.MapFrom(src => src.DimContactInformations))
            //.ForMember(dst => dst.OtherIdentifierService, opt => opt.MapFrom(src => src.))
            //.ForMember(dst => dst.ServiceUrn, opt => opt.MapFrom(src => src.))
            .ForMember(dst => dst.ServiceName, opt => opt.MapFrom(
                src => src.DimDescriptiveItems.Where(di => di.DescriptiveItemType == DescriptiveItemType_Name)))
            .ForMember(dst => dst.ServiceDescription, opt => opt.MapFrom(
                src => src.DimDescriptiveItems.Where(di => di.DescriptiveItemType == DescriptiveItemType_Description)))
            .ForMember(dst => dst.ServiceHomepage, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Homepage)));

        CreateProjection<DimContactInformation, ContactInformation>()
            .ForMember(dst => dst.ContactInformationContent, opt => opt.MapFrom(src => src.ContactInformationContent))
            .ForMember(dst => dst.ContactInformationType, opt => opt.MapFrom(src => ConvertToContactInformationType(src.ContactInformationType)))
            .ForMember(dst => dst.ContactInformationLabel, opt => opt.MapFrom(src => src.ContactName));
    }

    private static LanguageCode? ConvertToLanguageCode(string? descriptiveItemLanguage)
    {
        return Enum.TryParse<LanguageCode>(descriptiveItemLanguage, true, out var result) ? result : (LanguageCode?)null;
    }

    private static ContactInformationType? ConvertToContactInformationType(string? contactInformationType)
    {
        return Enum.TryParse<ContactInformationType>(contactInformationType, true, out var result) ? result : (ContactInformationType?)null;
    }
}
