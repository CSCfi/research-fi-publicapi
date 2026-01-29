using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using Infrastructure = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;

namespace CSC.PublicApi.Repositories.Maps;

public class InfrastructureProfile : Profile
{
    private const string DimPid_PidType_Urn = "URN";
    private const string DimPid_PidType_ROR = "RORID";
    private const string DimPid_PidType_BusinessID = "BusinessID";
    private const string DescriptiveItemType_Name = "name";
    private const string DescriptiveItemType_Description = "description";
    private const string DescriptiveItemType_ObtainInstruction = "obtain_instruction";
    private const string DescriptiveItemType_ScientificDescription = "scientific_description";
    private const string WebLinkType_Homepage = "homepage";
    private const string WebLinkType_TermsOfUse = "terms_of_use";
    private const string WebLinkType_EndUserGuide = "endUserGuide";
    private const string WebLinkType_PrivacyPolicy = "privacy_policy";
    private const string WebLinkType_Booking = "booking";
    private const string ContactInformationType_Email = "email";
    private const string ContactInformationType_PhoneNumber = "phone_number";
    private const string ContactInformationType_PostalAddress = "postal_address";
    private const string ContactInformationType_VisitingAddress = "visiting_address";
    private const string DimReferencedata_CodeScheme_ESFRI_Domain = "ESFRI-Domain";
    private const string DimReferencedata_CodeScheme_FieldOfScience = "Tieteenala2010";
    private const string DimReferencedata_CodeScheme_Infrapalvelu_Kayttaja = "infrapalvelu-kayttaja";
    private const string DimReferencedata_CodeScheme_Infrapalvelu_Kohderyhma = "infrapalvelu-kohderyhma";
    private const string ContributionType_Participant = "organization_participate_infrastructure";

    public InfrastructureProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateProjection<DimInfrastructure, CSC.PublicApi.Service.Models.Infrastructure.Infrastructure>()
            .ForMember(dst => dst.ExportSortId, opt => opt.MapFrom(src => (long)src.Id))
            // Local identifier - handled in InfrastructureIndexRepository
            .ForMember(dst => dst.LocalIdentifier, opt => opt.MapFrom(src => src.LocalIdentifier))
            // Pids - handled in InfrastructureIndexRepository
            .ForMember(dst => dst.Pids, opt => opt.MapFrom(src => src.DimPids))
            // InfraIdentifier
            .ForMember(dst => dst.InfraIdentifier, opt => opt.Ignore()) // Handled in InfrastructureIndexRepository
            // Acronym
            .ForMember(dst => dst.Acronym, opt => opt.MapFrom(src => src.Acronym))
            // Start date
            .ForMember(dst => dst.InfraStartsOn, opt => opt.MapFrom(src => src.DimStartDate != -1 ? new InfraDate(src.DimStartDateNavigation.Year, src.DimStartDateNavigation.Month, src.DimStartDateNavigation.Day) : null))
            // End date
            .ForMember(dst => dst.InfraEndsOn, opt => opt.MapFrom(src => src.DimEndDate != -1 ? new InfraDate(src.DimEndDateNavigation.Year, src.DimEndDateNavigation.Month, src.DimEndDateNavigation.Day) : null))
            // On the academy of Finland road map
            .ForMember(dst => dst.FinlandRoadmapInfrastructure, opt => opt.MapFrom(src => src.FinlandRoadmap))
            // Name
            .ForMember(dst => dst.InfraName, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_Name)))
            // Description
            .ForMember(dst => dst.InfraDescription, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_Description)))
            // ESFRI
            .ForMember(dst => dst.Esfri, opt => opt.MapFrom(src => src.FactReferencedata.Where(fr => fr.DimReferencedata.CodeScheme == DimReferencedata_CodeScheme_ESFRI_Domain).Select(fr => fr.DimReferencedata)))
            // Web link - Homepage
            .ForMember(dst => dst.InfraHomepage, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Homepage)))
            // Services
            .ForMember(dst => dst.IsComposedOf, opt => opt.MapFrom(src => src.DimServices))
            // Contact information
            .ForMember(dst => dst.InfraContactInformation, opt => opt.MapFrom(src => src.DimContactInformations))
            // Infrastructure network
            // Collect only from "FactRelationFromInfrastructures", do not collect "FactRelationToInfrastructures"
            .ForMember(dst => dst.InfraNetwork,
                opt => opt.MapFrom(src => src.FactRelationFromInfrastructures
                    .Select(rel => new InfrastructureNetwork
                    {
                        InfranetworkRelationType = rel.RelationTypeCodeNavigation != null
                            ? new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                            {
                                CodeValue = rel.RelationTypeCodeNavigation.CodeValue,
                                CodeDescription = new LanguageVariant
                                {
                                    Fi = rel.RelationTypeCodeNavigation.NameFi,
                                    Sv = rel.RelationTypeCodeNavigation.NameSv,
                                    En = rel.RelationTypeCodeNavigation.NameEn
                                }
                            }
                            : null,
                        RelationValid = rel.ValidRelation,
                        Pids = rel.ToInfrastructure.DimPids // Collect Pids, processed in InfrastructureIndexRepository
                            .Select(dp => new PersistentIdentifier
                            {
                                Content = dp.PidContent,
                                Type = dp.PidType
                            }).ToList(),
                        RelationToInfra = null, // Handled in InfrastructureIndexRepository
                        RelationToInternationalInfra = null // TODO: Handled in InfrastructureIndexRepository
                    }).ToList()
                )
            )
            // Field of science
            .ForMember(dst => dst.FieldOfScience, opt => opt.MapFrom(src => src.FactDimReferencedataFieldOfSciences.ToList().Select(f => f.DimReferencedata)))
            // Organization - participant
            .ForMember(dst => dst.OrganizationParticipatesInfrastructure, opt => opt.MapFrom(src => src.FactContributions
                .Where(fc => fc.ContributionType == ContributionType_Participant && fc.DimOrganizationId != -1)
                .Select(fc => fc.DimOrganization)))
            // Organization - responsible
           .ForMember(dst => dst.ResponsibleOrganization, opt => opt.MapFrom(src => src.ResponsibleOrganizationId != -1 ? src.ResponsibleOrganization : null))
           // Infrastructure researchfi URL - handled in InfrastructureIndexRepository
           .ForMember(dst => dst.InfraResearchfiURL, opt => opt.Ignore());



        CreateProjection<DimService, CSC.PublicApi.Service.Models.Infrastructure.Service>()
            // Export sort id
            .ForMember(dst => dst.ExportSortId, opt => opt.MapFrom(src => (long)src.Id))
            // Local identifier
            .ForMember(dst => dst.LocalIdentifier, opt => opt.MapFrom(src => src.LocalIdentifier))
            // Pids - handled in InfrastructureServiceIndexRepository. Set to null if empty.
            .ForMember(dst => dst.Pids, opt => opt.MapFrom(src => src.DimPids))
            // Service name
            .ForMember(dst => dst.ServiceName, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_Name)))
            // Service contact information
            .ForMember(dst => dst.ServiceContactInformation, opt => opt.MapFrom(src => src.DimContactInformations))
            // Service description
            .ForMember(dst => dst.ServiceDescription, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_Description)))
            // Service homepage
            .ForMember(dst => dst.ServiceHomepage, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Homepage)))
            // Service identifier - handled in InfrastructureServiceIndexRepository
            .ForMember(dst => dst.ServiceIdentifier, opt => opt.Ignore())
            // Service is part of infrastructure - handled in InfrastructureServiceIndexRepository
            .ForMember(dst => dst.IsPartOf, opt => opt.Ignore())
            // DimInfrastructureId
            .ForMember(dst => dst.DimInfrastructureId, opt => opt.MapFrom(src => src.DimInfrastructureId))
            // Start date
             .ForMember(dst => dst.ServiceStartsOn, opt => opt.MapFrom(src => src.StartDate != -1 ? new InfraDate(src.StartDateNavigation.Year, src.StartDateNavigation.Month, src.StartDateNavigation.Day) : null))
            // End date
            .ForMember(dst => dst.ServiceEndsOn, opt => opt.MapFrom(src => src.EndDate != -1 ? new InfraDate(src.EndDateNavigation.Year, src.EndDateNavigation.Month, src.EndDateNavigation.Day) : null))
            // Service user role
            .ForMember(dst => dst.ServiceUserRole, opt => opt.MapFrom(src => src.FactReferencedata
                .Where(fr => fr.DimReferencedata.CodeScheme == DimReferencedata_CodeScheme_Infrapalvelu_Kayttaja).Select(fr => fr.DimReferencedata)))
            // Service end user guide
            .ForMember(dst => dst.ServiceEndUserGuide, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_EndUserGuide)))
            // Service obtain
            .ForMember(dst => dst.ServiceObtain, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_ObtainInstruction)))
            // Service booking link
            .ForMember(dst => dst.ServiceBookingLink, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Booking)))
            // Service target segment
            .ForMember(dst => dst.ServiceTargetSegment, opt => opt.MapFrom(src => src.FactReferencedata
                .Where(fr => fr.DimReferencedata.CodeScheme == DimReferencedata_CodeScheme_Infrapalvelu_Kohderyhma).Select(fr => fr.DimReferencedata)))
            // Service researchfi URL - handled in InfrastructureServiceIndexRepository
            .ForMember(dst => dst.ServiceResearchfiURL, opt => opt.Ignore())
            // Service privacy policy
            .ForMember(dst => dst.ServicePrivacyPolicy, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_PrivacyPolicy)))
            // Service terms of use
            .ForMember(dst => dst.ServiceTermsOfUse, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_TermsOfUse)));


        // CreateProjection<DimInfrastructure, ServiceInfrastructure>()
        //     // Pids - handled in InfrastructureServiceIndexRepository
        //     .ForMember(dst => dst.Pids, opt => opt.MapFrom(src => src.DimPids))
        //     // Infrastructure identifier - handled in InfrastructureServiceIndexRepository
        //     .ForMember(dst => dst.InfraIdentifier, opt => opt.Ignore())
        //     // Responsible organization
        //     .ForMember(dst => dst.ResponsibleOrganization, opt => opt.MapFrom(src => src.ResponsibleOrganization))
        //     // Organization - participant
        //     .ForMember(dst => dst.OrganizationParticipatesInfrastructure, opt => opt.MapFrom(src => src.FactContributions
        //         .Where(fc => fc.ContributionType == ContributionType_Participant && fc.DimOrganizationId != -1)
        //         .Select(fc => fc.DimOrganization)))
        //     // Name
        //     .ForMember(dst => dst.InfraName, opt => opt.MapFrom(src => src.DimDescriptiveItems
        //         .Where(di => di.DescriptiveItemType == DescriptiveItemType_Name)))
        //     // ESFRI
        //     .ForMember(dst => dst.Esfri, opt => opt.MapFrom(src => src.FactReferencedata
        //         .Where(fr => fr.DimReferencedata.CodeScheme == DimReferencedata_CodeScheme_ESFRI_Domain).Select(fr => fr.DimReferencedata)));

        CreateProjection<DimOrganization, ResearchOrganization>()
            .ForMember(dst => dst.DimOrganizationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.OrganizationName, opt => opt.MapFrom(src => new LanguageVariant
                {
                    Fi = src.NameFi,
                    Sv = src.NameSv,
                    En = src.NameEn
                }))
            .ForMember(dst => dst.OrganizationIdentifierROR, opt => opt.Ignore()) // handled in HandleOrganizations()
            .ForMember(dst => dst.OrganizationIdentifier, opt => opt.Ignore()); // handled in HandleOrganizations()

        CreateProjection<DimReferencedatum, CSC.PublicApi.Service.Models.Infrastructure.ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.CodeValue, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.CodeDescription, opt => opt.MapFrom(src => new LanguageVariant
            {
                Fi = src.NameFi,
                Sv = src.NameSv,
                En = src.NameEn
            }));

        CreateProjection<DimDescriptiveItem, DescriptiveText>()
            .ForMember(dst => dst.TextContent, opt => opt.MapFrom(src => src.DescriptiveItem))
            .ForMember(dst => dst.TextLanguage, opt => opt.MapFrom(src => src.DescriptiveItemLanguage));

        CreateProjection<DimWebLink, Weblink>()
            .ForMember(dst => dst.WeblinkLanguage, opt => opt.MapFrom(src => src.LanguageVariant))
            .ForMember(dst => dst.WeblinkURL, opt => opt.MapFrom(src => src.Url));

        CreateProjection<DimService, InfrastructureService>()
            // Local identifier - handled in InfrastructureIndexRepository
            .ForMember(dst => dst.LocalIdentifier, opt => opt.MapFrom(src => src.LocalIdentifier))
            // Pids - handled in InfrastructureServiceIndexRepository
            .ForMember(dst => dst.Pids, opt => opt.MapFrom(src => src.DimPids))
            .ForMember(dst => dst.ServiceContactInformation, opt => opt.MapFrom(src => src.DimContactInformations))
            .ForMember(dst => dst.ServiceName, opt => opt.MapFrom(
                src => src.DimDescriptiveItems.Where(di => di.DescriptiveItemType == DescriptiveItemType_Name)))
            .ForMember(dst => dst.ServiceDescription, opt => opt.MapFrom(
                src => src.DimDescriptiveItems.Where(di => di.DescriptiveItemType == DescriptiveItemType_Description)))
            .ForMember(dst => dst.ServiceHomepage, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Homepage)))
            .ForMember(dst => dst.ServiceIdentifier, opt => opt.Ignore()) // Handled in InfrastructureIndexRepository
            .ForMember(dst => dst.ServiceUserRole, opt => opt.MapFrom(src => src.FactReferencedata
                .Where(fr => fr.DimReferencedata.CodeScheme == DimReferencedata_CodeScheme_Infrapalvelu_Kayttaja).Select(fr => fr.DimReferencedata)))
            .ForMember(dst => dst.ServiceEndUserGuide, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_EndUserGuide)))
            .ForMember(dst => dst.ServiceObtain, opt => opt.MapFrom(src => src.DimDescriptiveItems
                .Where(di => di.DescriptiveItemType == DescriptiveItemType_ObtainInstruction)))
            .ForMember(dst => dst.ServiceBookingLink, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_Booking)))
            .ForMember(dst => dst.ServiceTargetSegment, opt => opt.MapFrom(src => src.FactReferencedata
                .Where(fr => fr.DimReferencedata.CodeScheme == DimReferencedata_CodeScheme_Infrapalvelu_Kohderyhma).Select(fr => fr.DimReferencedata)))
            .ForMember(dst => dst.ServiceResearchfiURL, opt => opt.Ignore())
            .ForMember(dst => dst.ServicePrivacyPolicy, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_PrivacyPolicy)))
            .ForMember(dst => dst.ServiceTermsOfUse, opt => opt.MapFrom(src => src.DimWebLinks
                .Where(wl => wl.LinkType == WebLinkType_TermsOfUse)))
            .ForMember(dst => dst.ServiceStartsOn, opt => opt.MapFrom(src => src.StartDate != -1 ? new InfraDate(src.StartDateNavigation.Year, src.StartDateNavigation.Month, src.StartDateNavigation.Day) : null))
            .ForMember(dst => dst.ServiceEndsOn, opt => opt.MapFrom(src => src.EndDate != -1 ? new InfraDate(src.EndDateNavigation.Year, src.EndDateNavigation.Month, src.EndDateNavigation.Day) : null));

        CreateProjection<DimContactInformation, ContactInformation>()
            .ForMember(dst => dst.ContactInformationLabel, opt => opt.MapFrom(src => src.ContactLabel))
            .ForMember(dst => dst.Phone, opt => opt.MapFrom(src => src.DimTelephoneNumbers.Select(tn => tn.TelephoneNumber)))
            .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.DimEmailAddrresses.Select(em => em.Email)))
            .ForMember(dst => dst.VisitingAddress, opt => opt.MapFrom(src => src.DimAddresses
                .FirstOrDefault()));

        CreateProjection<DimAddress, Address>()
            .ForMember(dst => dst.Premise, opt => opt.MapFrom(src => src.Premise))
            .ForMember(dst => dst.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dst => dst.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            .ForMember(dst => dst.Country, opt => opt.MapFrom(src => src.CountryCodeNavigation))
            .ForMember(dst => dst.Locality, opt => opt.MapFrom(src => new LanguageVariant
            {
                Fi = src.LocalityFi,
                Sv = src.LocalitySv,
                En = src.LocalityEn
            }));

        CreateProjection<DimDate, DateTime?>()
            .ConvertUsing(dimDate => dimDate.Id == -1 ? null : new DateTime(dimDate.Year, dimDate.Month, dimDate.Day));

        CreateProjection<DimPid, Service.Models.PersistentIdentifier>()
            .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.PidContent))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.PidType));
    }
}