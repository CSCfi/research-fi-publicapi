using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using FluentAssertions;
using Xunit;
using InfrastructureServiceModel = CSC.PublicApi.Service.Models.Infrastructure.Service;

namespace CSC.PublicApi.Indexer.Tests.Maps;

public class InfrastructureServiceProfileTest
{
    private readonly IMapper _mapper;

    public InfrastructureServiceProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<InfrastructureProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void ProjectTo_DimService_ShouldBeMappedToInfrastructureService()
    {
        // Arrange
        var entity = GetInfrastructureServiceEntity();
        var model = GetInfrastructureServiceModel();

        // Act
        var result = Act_Map(entity);

        // Assert
        result.Should().BeEquivalentTo(model, options => options);
    }



    private InfrastructureServiceModel Act_Map(DimService dbEntity)
    {
        var entityQueryable = new List<DimService>
        {
            dbEntity
        }.AsQueryable();

        var modelCollection = _mapper.ProjectTo<InfrastructureServiceModel>(entityQueryable);

        return modelCollection.Single();
    }


    private static DimService GetInfrastructureServiceEntity()
    {
        return new DimService
        {
            StartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2021, Month = 11, Day = 1, SourceId = "SRC123" },
            EndDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 2, Year = 2022, Month = 12, Day = -1, SourceId = "SRC123" },
            DimPids = new List<CSC.PublicApi.DatabaseContext.Entities.DimPid> {
                new CSC.PublicApi.DatabaseContext.Entities.DimPid {
                    Id = 1,
                    PidType = "urn",
                    PidContent = "urn:nbn:fi:12345"
                }
            },
            DimDescriptiveItems = new List<CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem> {
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 1,
                    DescriptiveItem = "Test service name 1",
                    DescriptiveItemType = "name",
                    DescriptiveItemLanguage = "en",
                    SourceId = "SRC123",
                    DimServiceId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2021, Month = 11, Day = 1, SourceId = "SRC123" }
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 2,
                    DescriptiveItem = "Test service name 2",
                    DescriptiveItemType = "name",
                    DescriptiveItemLanguage = "fi",
                    SourceId = "SRC123",
                    DimServiceId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 2, Year = 2022, Month = 12, Day = 2, SourceId = "SRC123" }
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 3,
                    DescriptiveItem = "Test service description 1",
                    DescriptiveItemType = "description",
                    DescriptiveItemLanguage = "en",
                    SourceId = "SRC123",
                    DimServiceId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2021, Month = 11, Day = 1, SourceId = "SRC123" }
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 4,
                    DescriptiveItem = "Test service description 2",
                    DescriptiveItemType = "description",
                    DescriptiveItemLanguage = "fi",
                    SourceId = "SRC123",
                    DimServiceId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2022, Month = 12, Day = 2, SourceId = "SRC123" }
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 5,
                    DescriptiveItem = "Test service obtain instruction 1",
                    DescriptiveItemType = "obtain_instruction",
                    DescriptiveItemLanguage = "fi",
                    SourceId = "SRC123",
                    DimServiceId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2021, Month = 11, Day = 1, SourceId = "SRC123" }
                }
            },
            DimContactInformations = new List<DimContactInformation>
            {
                new DimContactInformation
                {
                    Id = 1,
                    ContactLabel = "contact 1 label",
                    DimAddresses = new List<DimAddress>
                    {
                        new DimAddress
                        {
                            Id = 1,
                            Street = "address 1 street",
                            Premise = "address 1 premise",
                            PostOfficeBox = "address 1 po box",
                            PostalCode = "address 1 postalcode",
                            LocalityFi = "address 1 locality FI",
                            LocalitySv = "address 1 locality SV",
                            LocalityEn = "address 1 locality EN",
                            CountryCode = 246,
                            AddressType = "address 1 type",
                            CountryCodeNavigation = new DimReferencedatum
                            {
                                CodeScheme = "country",
                                CodeValue = "246",
                                NameFi = "country 1 name FI",
                                NameEn = "country 1 name EN",
                                NameSv = "country 1 name SV"
                            }
                        }
                    },
                    DimEmailAddrresses = new List<DimEmailAddrress>
                    {
                        new DimEmailAddrress{ Email = "email1@example.com" },
                        new DimEmailAddrress{ Email = "email2@example.com" }
                    },
                    DimTelephoneNumbers = new List<DimTelephoneNumber>
                    {
                        new DimTelephoneNumber{ TelephoneNumber = "+358401234567" },
                        new DimTelephoneNumber{ TelephoneNumber = "+358409876543" }   
                    }
                }
            },
            DimWebLinks = new List<CSC.PublicApi.DatabaseContext.Entities.DimWebLink> {
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "https://testservice-homepage-1.fi",
                    LinkLabel = "homepage label 1",
                    LinkType = "homepage",
                    LanguageVariant = "en",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "https://testservice-homepage-2.fi",
                    LinkLabel = "homepage label 2",
                    LinkType = "homepage",
                    LanguageVariant = "fi",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-enduserguide-1.fi",
                    LinkLabel = "end user guide label 1",
                    LinkType = "endUserGuide",
                    LanguageVariant = "en",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-enduserguide-2.fi",
                    LinkLabel = "end user guide label 2",
                    LinkType = "endUserGuide",
                    LanguageVariant = "fi",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-booking-1.fi",
                    LinkLabel = "booking label 1",
                    LinkType = "booking",
                    LanguageVariant = "en",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-booking-2.fi",
                    LinkLabel = "booking label 2",
                    LinkType = "booking",
                    LanguageVariant = "fi",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-privacy-policy-1.fi",
                    LinkLabel = "privacy policy label 1",
                    LinkType = "privacy_policy",
                    LanguageVariant = "en",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-privacy-policy-2.fi",
                    LinkLabel = "privacy policy label 2",
                    LinkType = "privacy_policy",
                    LanguageVariant = "fi",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-terms-of-use-1.fi",
                    LinkLabel = "terms of use label 1",
                    LinkType = "terms_of_use",
                    LanguageVariant = "en",
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Url = "service-terms-of-use-2.fi",
                    LinkLabel = "terms of use label 2",
                    LinkType = "terms_of_use",
                    LanguageVariant = "fi",
                }
            },
            FactReferencedata = new List<FactReferencedatum>
            {
                new FactReferencedatum
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "infrapalvelu-kayttaja",
                        CodeValue = "service-user-role-1-codevalue",
                        NameFi = "service-user-role-1-code-description-fi",
                        NameEn = "service-user-role-1-code-description-en",
                        NameSv = "service-user-role-1-code-description-sv"
                    }
                },
                new FactReferencedatum
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "infrapalvelu-kayttaja",
                        CodeValue = "service-user-role-2-codevalue",
                        NameFi = "service-user-role-2-code-description-fi",
                        NameEn = "service-user-role-2-code-description-en",
                        NameSv = "service-user-role-2-code-description-sv"
                    }
                },
                new FactReferencedatum
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "infrapalvelu-kohderyhma",
                        CodeValue = "service-target-segment-1-codevalue",
                        NameFi = "service-target-segment-1-code-description-fi",
                        NameEn = "service-target-segment-1-code-description-en",
                        NameSv = "service-target-segment-1-code-description-sv"
                    }
                },
                new FactReferencedatum
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "infrapalvelu-kohderyhma",
                        CodeValue = "service-target-segment-2-codevalue",
                        NameFi = "service-target-segment-2-code-description-fi",
                        NameEn = "service-target-segment-2-code-description-en",
                        NameSv = "service-target-segment-2-code-description-sv"
                    }
                },
                new FactReferencedatum
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "ESFRI-Domain",
                        CodeValue = "esfri 1 code value",
                        NameFi = "esfri 1 name FI",
                        NameEn = "esfri 1 name EN",
                        NameSv = "esfri 1 name SV"
                    }
                },
                new FactReferencedatum
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "ESFRI-Domain",
                        CodeValue = "esfri 2 code value",
                        NameFi = "esfri 2 name FI",
                        NameEn = "esfri 2 name EN",
                        NameSv = "esfri 2 name SV"
                    }
                }
            },
            DimInfrastructure = new DimInfrastructure
            {
                DimPids = new List<DimPid>
                {
                    new DimPid
                    {
                        PidType = "pid-type-1",
                        PidContent = "infranetwork-infra-1-pid-content-1"
                    },
                    new DimPid
                    {
                        PidType = "pid-type-2",
                        PidContent = "infranetwork-infra-1-pid-content-2"
                    }
                },
                LocalIdentifier = "infranetwork-infra-1-infralocalidentifier",
                DimDescriptiveItems = new List<DimDescriptiveItem>
                {
                    new DimDescriptiveItem
                    {
                        DescriptiveItem = "infranetwork-infra-1-infraname-descriptive",
                        DescriptiveItemType = "name",
                        DescriptiveItemLanguage = "fi"
                    },
                    new DimDescriptiveItem
                    {
                        DescriptiveItem = "infranetwork-infra-1-infraname-descriptive-en",
                        DescriptiveItemType = "name",
                        DescriptiveItemLanguage = "en"
                    }
                },
                FactReferencedata = new List<FactReferencedatum>
                {
                    new FactReferencedatum
                    {
                        DimReferencedata = new DimReferencedatum
                        {
                            CodeScheme = "ESFRI-Domain",
                            CodeValue = "esfri 1 code value",
                            NameFi = "esfri 1 name FI",
                            NameEn = "esfri 1 name EN",
                            NameSv = "esfri 1 name SV"
                        }
                    },
                    new FactReferencedatum
                    {
                        DimReferencedata = new DimReferencedatum
                        {
                            CodeScheme = "ESFRI-Domain",
                            CodeValue = "esfri 2 code value",
                            NameFi = "esfri 2 name FI",
                            NameEn = "esfri 2 name EN",
                            NameSv = "esfri 2 name SV"
                        }
                    }             
                },
                ResponsibleOrganization = new DimOrganization
                {
                    Id = 12345,
                    NameFi = "org3-name-fi",
                    NameEn = "org3-name-en",
                    NameSv = "org3-name-sv"
                }
            }
        };
    }


    private static InfrastructureServiceModel GetInfrastructureServiceModel()
    {
        return new InfrastructureServiceModel
        {
            ServiceName = new List<DescriptiveText> {
                new DescriptiveText { TextContent = "Test service name 1", TextLanguage = "en" },
                new DescriptiveText { TextContent = "Test service name 2", TextLanguage = "fi" }
            },
            ServiceDescription = new List<DescriptiveText> {
                new DescriptiveText { TextContent = "Test service description 1", TextLanguage = "en" },
                new DescriptiveText { TextContent = "Test service description 2", TextLanguage = "fi" }
            },
            ServiceStartsOn = new InfraDate(year: 2021, month: 11, day: 1),
            ServiceEndsOn = new InfraDate(year: 2022, month: 12, day: null),
            Pids = new List<CSC.PublicApi.Service.Models.PersistentIdentifier>
            {
                new PersistentIdentifier
                {
                    Type = "urn",
                    Content = "urn:nbn:fi:12345"
                }
            },
            ServiceHomepage = new List<Weblink> {
                new Weblink { WeblinkURL = "https://testservice-homepage-1.fi", WeblinkLanguage = "en" },
                new Weblink { WeblinkURL = "https://testservice-homepage-2.fi", WeblinkLanguage = "fi" }
            },
            ServiceContactInformation = new List<ContactInformation>
            {
                new ContactInformation
                {
                    ContactInformationLabel = "contact 1 label",
                    Phone = new List<string>
                    {
                        "+358401234567",
                        "+358409876543"
                    },
                    Email = new List<string>
                    {
                        "email1@example.com",
                        "email2@example.com"
                    },
                    VisitingAddress = new Service.Models.Infrastructure.Address
                    {
                        Street = "address 1 street",
                        Premise = "address 1 premise",
                        PostalCode = "address 1 postalcode",
                        Locality = new LanguageVariant
                        {
                            Fi = "address 1 locality FI",
                            Sv = "address 1 locality SV",
                            En = "address 1 locality EN"
                        },
                        Country = new Service.Models.Infrastructure.ReferenceData
                        {
                            CodeValue = "246",
                            CodeDescription = new Service.Models.Infrastructure.LanguageVariant
                            {
                                Fi = "country 1 name FI",
                                En = "country 1 name EN",
                                Sv = "country 1 name SV"
                            }
                        }
                    }
                }
            },
            ServiceUserRole = new List<CSC.PublicApi.Service.Models.Infrastructure.ReferenceData>
            {
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeValue = "service-user-role-1-codevalue",
                    CodeDescription = new LanguageVariant
                    {
                        Fi = "service-user-role-1-code-description-fi",
                        En = "service-user-role-1-code-description-en",
                        Sv = "service-user-role-1-code-description-sv"
                    }
                },
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeValue = "service-user-role-2-codevalue",
                    CodeDescription = new LanguageVariant
                    {
                        Fi = "service-user-role-2-code-description-fi",
                        En = "service-user-role-2-code-description-en",
                        Sv = "service-user-role-2-code-description-sv"
                    }
                }
            },
            ServiceEndUserGuide = new List<Weblink>
            {
                new Weblink { WeblinkURL = "service-enduserguide-1.fi", WeblinkLanguage = "en" },
                new Weblink { WeblinkURL = "service-enduserguide-2.fi", WeblinkLanguage = "fi" }
            },
            IsPartOf = null, // Handled in InfrastructureServiceIndexRepository
            ServiceIdentifier = null,
            ServiceObtain = new List<DescriptiveText> {
                new DescriptiveText {
                    TextContent = "Test service obtain instruction 1", 
                    TextLanguage = "fi"
                }
            },
            ServiceBookingLink = new List<Weblink>
            {
                new Weblink { WeblinkURL = "service-booking-1.fi", WeblinkLanguage = "en" },
                new Weblink { WeblinkURL = "service-booking-2.fi", WeblinkLanguage = "fi" }
            },
            ServiceTargetSegment = new List<CSC.PublicApi.Service.Models.Infrastructure.ReferenceData>
            {
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeValue = "service-target-segment-1-codevalue",
                    CodeDescription = new LanguageVariant
                    {
                        Fi = "service-target-segment-1-code-description-fi",
                        En = "service-target-segment-1-code-description-en",
                        Sv = "service-target-segment-1-code-description-sv"
                    }
                },
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeValue = "service-target-segment-2-codevalue",
                    CodeDescription = new LanguageVariant
                    {
                        Fi = "service-target-segment-2-code-description-fi",
                        En = "service-target-segment-2-code-description-en",
                        Sv = "service-target-segment-2-code-description-sv"
                    }
                }
            },
            ServiceResearchfiURL = null,
            ServicePrivacyPolicy = new List<Weblink>
            {
                new Weblink { WeblinkURL = "service-privacy-policy-1.fi", WeblinkLanguage = "en" },
                new Weblink { WeblinkURL = "service-privacy-policy-2.fi", WeblinkLanguage = "fi" }
            },
            ServiceTermsOfUse = new List<Weblink>
            {
                new Weblink { WeblinkURL = "service-terms-of-use-1.fi", WeblinkLanguage = "en" },
                new Weblink { WeblinkURL = "service-terms-of-use-2.fi", WeblinkLanguage = "fi" }
            }
        };
    }
}