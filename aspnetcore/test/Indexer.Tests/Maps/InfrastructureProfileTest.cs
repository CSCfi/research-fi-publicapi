using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using FluentAssertions;
using Xunit;
using InfrastructureModel = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;

namespace CSC.PublicApi.Indexer.Tests.Maps;

public class InfrastructureProfileTest
{
    private readonly IMapper _mapper;

    public InfrastructureProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<InfrastructureProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void ProjectTo_DimInfrastructure_ShouldBeMappedToInfrastructure()
    {
        // Arrange
        var entity = GetInfrastructureEntity();
        var model = GetInfrastructureModel();

        // Act
        var result = Act_Map(entity);

        // Assert
        result.Should().BeEquivalentTo(model, options => options);
    }



    private InfrastructureModel Act_Map(DimInfrastructure dbEntity)
    {
        var entityQueryable = new List<DimInfrastructure>
        {
            dbEntity
        }.AsQueryable();

        var modelCollection = _mapper.ProjectTo<InfrastructureModel>(entityQueryable);

        return modelCollection.Single();
    }


    private static DimInfrastructure GetInfrastructureEntity()
    {
        return new DimInfrastructure
        {
            Id = 1,
            Acronym = "Test acronym",
            FinlandRoadmap = true,
            SourceId = "SRC123",
            SourceDescription = "Test source description",
            Created = new DateTime(2022, 1, 1),
            Modified = new DateTime(2023, 1, 1),
            DimRegisteredDataSourceId = 10,
            ResponsibleOrganizationId = 20,
            DimStartDate = 20220101,
            DimEndDate = 20231231,
            LocalIdentifier = "INFRA-LOCAL-1",
            DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate {
                Id = 1, Year = 2022, Month = 11, Day = 1, SourceId = "SRC123"
            },
            DimEndDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate {
                Id = 2, Year = 2023, Month = 12, Day = -1, SourceId = "SRC123"
            },
            DimDescriptiveItems = new List<CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem> {
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 1,
                    DescriptiveItem = "Test infra name 1",
                    DescriptiveItemType = "name",
                    DescriptiveItemLanguage = "en",
                    SourceId = "SRC123",
                    DimInfrastructureId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2021, Month = 11, Day = 1, SourceId = "SRC123" }
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 1,
                    DescriptiveItem = "Test infra name 2",
                    DescriptiveItemType = "name",
                    DescriptiveItemLanguage = "fi",
                    SourceId = "SRC123",
                    DimInfrastructureId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 2, Year = 2022, Month = 12, Day = 2, SourceId = "SRC123" }
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 2,
                    DescriptiveItem = "Test infra description 1",
                    DescriptiveItemType = "description",
                    DescriptiveItemLanguage = "en",
                    SourceId = "SRC123",
                    DimInfrastructureId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2021, Month = 11, Day = 1, SourceId = "SRC123" }
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimDescriptiveItem {
                    Id = 2,
                    DescriptiveItem = "Test infra description 2",
                    DescriptiveItemType = "description",
                    DescriptiveItemLanguage = "fi",
                    SourceId = "SRC123",
                    DimInfrastructureId = 1,
                    DimStartDateNavigation = new CSC.PublicApi.DatabaseContext.Entities.DimDate { Id = 1, Year = 2022, Month = 12, Day = 2, SourceId = "SRC123" }
                }
            },
            DimWebLinks = new List<CSC.PublicApi.DatabaseContext.Entities.DimWebLink> {
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Id = 1,
                    Url = "https://testinfra-homepage-1.fi",
                    LinkLabel = "homepage label 1",
                    LinkType = "homepage",
                    LanguageVariant = "en",
                    SourceId = "SRC123"
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimWebLink {
                    Id = 2,
                    Url = "https://testinfra-homepage-2.fi",
                    LinkLabel = "homepage label 2",
                    LinkType = "homepage",
                    LanguageVariant = "fi",
                    SourceId = "SRC123"
                }
            },
            DimPids = new List<CSC.PublicApi.DatabaseContext.Entities.DimPid> {
                new CSC.PublicApi.DatabaseContext.Entities.DimPid {
                    Id = 1,
                    PidType = "urn",
                    PidContent = "urn:nbn:fi:12345"
                },
                new CSC.PublicApi.DatabaseContext.Entities.DimPid {
                    Id = 2,
                    PidType = "other-pid-type",
                    PidContent = "other-pid-content"
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
            FactRelationFromInfrastructures = new List<FactRelation>
            {
                new FactRelation
                {
                    RelationTypeCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "relation-typecode-codevalue",
                        NameFi = "relation-typecode-name-fi",
                        NameEn = "relation-typecode-name-en",
                        NameSv = "relation-typecode-name-sv",
                    },
                    ValidRelation = true,
                    ToInfrastructure = new DimInfrastructure
                    {
                        DimPids = new List<DimPid>
                        {
                            new DimPid
                            {
                                PidType = "infranetwork-infra-1-pid1-type",
                                PidContent = "infranetwork-infra-1-pid1-content"
                            },
                            new DimPid
                            {
                                PidType = "infranetwork-infra-1-pid2-type",
                                PidContent = "infranetwork-infra-1-pid2-content"
                            }
                        }
                    }
                }
            },
            FactReferencedata = new List<FactReferencedatum>
            {
                new FactReferencedatum
                {
                    DimReferencedataId = 12345,
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
                    DimReferencedataId = 12346,
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
            FactDimReferencedataFieldOfSciences = new List<FactDimReferencedataFieldOfScience>
            {
                new FactDimReferencedataFieldOfScience
                {
                    DimReferencedataId = 22345,
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "Tieteenala2010",
                        CodeValue = "fos 1 code value",
                        NameFi = "fos 1 name FI",
                        NameEn = "fos 1 name EN",
                        NameSv = "fos 1 name SV"
                    }
                },
                new FactDimReferencedataFieldOfScience
                {
                    DimReferencedataId = 22346,
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeScheme = "Tieteenala2010",
                        CodeValue = "fos 2 code value",
                        NameFi = "fos 2 name FI",
                        NameEn = "fos 2 name EN",
                        NameSv = "fos 2 name SV"
                    }
                }
            },
            ResponsibleOrganization = new DimOrganization
            {
                Id = 12345,
                NameFi = "Responsible organization name FI",
                NameEn = "Responsible organization name EN",
                NameSv = "Responsible organization name SV"
            },
            DimServices = new List<DimService>
            {
                new DimService
                {
                    Id = 1,
                    SourceId = "SRC-SERV-1",
                    Created = new DateTime(2022, 2, 2),
                    Modified = new DateTime(2023, 2, 2),
                    DimDescriptiveItems = new List<DimDescriptiveItem>
                    {
                        new DimDescriptiveItem
                        {
                            Id = 1,
                            DescriptiveItem = "infraservice-1-name-1",
                            DescriptiveItemType = "name",
                            DescriptiveItemLanguage = "en",
                            SourceId = "SRC123",
                            DimServiceId = 1
                        },
                        new DimDescriptiveItem
                        {
                            Id = 2,
                            DescriptiveItem = "infraservice-1-name-2",
                            DescriptiveItemType = "name",
                            DescriptiveItemLanguage = "fi",
                            SourceId = "SRC123",
                            DimServiceId = 1
                        },
                        new DimDescriptiveItem
                        {
                            Id = 3,
                            DescriptiveItem = "infraservice-1-description-1",
                            DescriptiveItemType = "description",
                            DescriptiveItemLanguage = "en",
                            SourceId = "SRC123",
                            DimServiceId = 1
                        },
                        new DimDescriptiveItem
                        {
                            Id = 4,
                            DescriptiveItem = "infraservice-1-description-2",
                            DescriptiveItemType = "description",
                            DescriptiveItemLanguage = "fi",
                            SourceId = "SRC123",
                            DimServiceId = 1
                        }
                    },
                    DimContactInformations = new List<DimContactInformation>
                    {
                        new DimContactInformation
                        {
                            Id = 2,
                            ContactLabel = "infraservice-1-contactinformation-label",
                            DimEmailAddrresses = new List<DimEmailAddrress>
                            {
                                new DimEmailAddrress{ Email = "infraservice-1-email1@example.com" },
                                new DimEmailAddrress{ Email = "infraservice-1-email2@example.com" }
                            },
                            DimTelephoneNumbers = new List<DimTelephoneNumber>
                            {
                                new DimTelephoneNumber{ TelephoneNumber = "+358401234567" },
                                new DimTelephoneNumber{ TelephoneNumber = "+358409876543" }
                            }
                        }
                    },
                    DimWebLinks = new List<DimWebLink>
                    {
                        new DimWebLink{ Id = 1, Url = "https://infraservice-1-homepage-1.fi", LinkLabel = "infraservice-1-homepage-label-1", LinkType = "homepage", LanguageVariant = "en", SourceId = "SRC123" },
                        new DimWebLink{ Id = 2, Url = "https://infraservice-1-homepage-2.fi", LinkLabel = "infraservice-1-homepage-label-2", LinkType = "homepage", LanguageVariant = "fi", SourceId = "SRC123" }
                    }
                },
                new DimService
                {
                    Id = 2,
                    SourceId = "SRC-SERV-2",
                    Created = new DateTime(2022, 3, 3),
                    Modified = new DateTime(2023, 3, 3),
                    DimDescriptiveItems = new List<DimDescriptiveItem>
                    {
                        new DimDescriptiveItem
                        {
                            Id = 1,
                            DescriptiveItem = "infraservice-2-name-1",
                            DescriptiveItemType = "name",
                            DescriptiveItemLanguage = "en",
                            SourceId = "SRC123",
                            DimServiceId = 2
                        },
                        new DimDescriptiveItem
                        {
                            Id = 2,
                            DescriptiveItem = "infraservice-2-name-2",
                            DescriptiveItemType = "name",
                            DescriptiveItemLanguage = "fi",
                            SourceId = "SRC123",
                            DimServiceId = 1
                        },
                        new DimDescriptiveItem
                        {
                            Id = 3,
                            DescriptiveItem = "infraservice-2-description-1",
                            DescriptiveItemType = "description",
                            DescriptiveItemLanguage = "en",
                            SourceId = "SRC123",
                            DimServiceId = 2
                        },
                        new DimDescriptiveItem
                        {
                            Id = 4,
                            DescriptiveItem = "infraservice-2-description-2",
                            DescriptiveItemType = "description",
                            DescriptiveItemLanguage = "fi",
                            SourceId = "SRC123",
                            DimServiceId = 2
                        }
                    },
                                        DimContactInformations = new List<DimContactInformation>
                    {
                    new DimContactInformation
                        {
                            Id = 4,
                            ContactLabel = "infraservice-2-contactinformation-label",
                            DimEmailAddrresses = new List<DimEmailAddrress>
                            {
                                new DimEmailAddrress{ Email = "infraservice-2-email1@example.com" },
                                new DimEmailAddrress{ Email = "infraservice-2-email2@example.com" }
                            },
                            DimTelephoneNumbers = new List<DimTelephoneNumber>
                            {
                                new DimTelephoneNumber{ TelephoneNumber = "+35899887766" },
                                new DimTelephoneNumber{ TelephoneNumber = "+35855443322" }
                            }
                        }
                    },
                    DimWebLinks = new List<DimWebLink>
                    {
                        new DimWebLink{ Id = 1, Url = "https://infraservice-2-homepage-1.fi", LinkLabel = "infraservice-2-homepage-label-1", LinkType = "homepage", LanguageVariant = "en", SourceId = "SRC123" },
                        new DimWebLink{ Id = 2, Url = "https://infraservice-2-homepage-2.fi", LinkLabel = "infraservice-2-homepage-label-2", LinkType = "homepage", LanguageVariant = "fi", SourceId = "SRC123" }
                    }
                }
            }
        };
    }


    private static InfrastructureModel GetInfrastructureModel()
    {
        return new InfrastructureModel
        {
            ExportSortId = 1,
            InfraName = new List<DescriptiveText> {
                new DescriptiveText { TextContent = "Test infra name 1", TextLanguage = "en" },
                new DescriptiveText { TextContent = "Test infra name 2", TextLanguage = "fi" }
            },
            InfraDescription = new List<DescriptiveText> {
                new DescriptiveText { TextContent = "Test infra description 1", TextLanguage = "en" },
                new DescriptiveText { TextContent = "Test infra description 2", TextLanguage = "fi" }
            },
            Acronym = "Test acronym",
            LocalIdentifier = "INFRA-LOCAL-1",
            FinlandRoadmapInfrastructure = true,
            InfraStartsOn = new InfraDate(year: 2022, month: 11, day: 1),
            InfraEndsOn = new InfraDate(year: 2023, month: 12, day: null),
            InfraHomepage = new List<Weblink> {
                new Weblink { WeblinkURL = "https://testinfra-homepage-1.fi", WeblinkLanguage = "en" },
                new Weblink { WeblinkURL = "https://testinfra-homepage-2.fi", WeblinkLanguage = "fi" }
            },
            InfraContactInformation = new List<ContactInformation>
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
                        LocalityName = new LanguageVariant
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
            Pids = new List<CSC.PublicApi.Service.Models.PersistentIdentifier>
            {
                new PersistentIdentifier
                {
                    Type = "urn",
                    Content = "urn:nbn:fi:12345"
                },
                new PersistentIdentifier
                {
                    Type = "other-pid-type",
                    Content = "other-pid-content"
                }
            },
            Esfri = new()
            {
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeDescription = new
                    CSC.PublicApi.Service.Models.Infrastructure.LanguageVariant
                    {
                        En = "esfri 1 name EN", 
                        Fi = "esfri 1 name FI", 
                        Sv = "esfri 1 name SV"
                    }, 
                    CodeValue = "esfri 1 code value"
                }, 
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeDescription = new
                    CSC.PublicApi.Service.Models.Infrastructure.LanguageVariant
                    {
                        En = "esfri 2 name EN", 
                        Fi = "esfri 2 name FI", 
                        Sv = "esfri 2 name SV"
                    }, 
                    CodeValue = "esfri 2 code value"
                }
            },
            FieldOfScience = new List<CSC.PublicApi.Service.Models.Infrastructure.ReferenceData>
            {
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeDescription = new
                    CSC.PublicApi.Service.Models.Infrastructure.LanguageVariant
                    {
                        En = "fos 1 name EN", 
                        Fi = "fos 1 name FI", 
                        Sv = "fos 1 name SV"
                    }, 
                    CodeValue = "fos 1 code value"
                }, 
                new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                {
                    CodeDescription = new
                    CSC.PublicApi.Service.Models.Infrastructure.LanguageVariant
                    {
                        En = "fos 2 name EN", 
                        Fi = "fos 2 name FI", 
                        Sv = "fos 2 name SV"
                    }, 
                    CodeValue = "fos 2 code value"
                }
            },
            IsComposedOf = new List<InfrastructureService>
            {
                new InfrastructureService
                {
                    ServiceContactInformation = new List<ContactInformation>
                    {
                        new ContactInformation
                        {
                            ContactInformationLabel = "infraservice-1-contactinformation-label",
                            Phone = new List<string>
                            {
                                "+358401234567",
                                "+358409876543"
                            },
                            Email = new List<string>
                            {
                                "infraservice-1-email1@example.com",
                                "infraservice-1-email2@example.com"
                            }
                        }
                    },
                    ServiceDescription = new List<DescriptiveText>
                    {
                        new DescriptiveText{ TextContent = "infraservice-1-description-1", TextLanguage = "en" },
                        new DescriptiveText{ TextContent = "infraservice-1-description-2", TextLanguage = "fi" }
                    },
                    ServiceName = new List<DescriptiveText>
                    {
                        new DescriptiveText{ TextContent = "infraservice-1-name-1", TextLanguage = "en" },
                        new DescriptiveText{ TextContent = "infraservice-1-name-2", TextLanguage = "fi" }
                    },
                    ServiceHomepage = new List<Weblink>
                    {
                        new Weblink{ WeblinkURL = "https://infraservice-1-homepage-1.fi", WeblinkLanguage = "en" },
                        new Weblink{ WeblinkURL = "https://infraservice-1-homepage-2.fi", WeblinkLanguage = "fi" }
                    }
                },
                new InfrastructureService
                {
                    ServiceContactInformation = new List<ContactInformation>
                    {
                        new ContactInformation
                        {
                            ContactInformationLabel = "infraservice-2-contactinformation-label",
                            Phone = new List<string>
                            {
                                "+35899887766",
                                "+35855443322"
                            },
                            Email = new List<string>
                            {
                                "infraservice-2-email1@example.com",
                                "infraservice-2-email2@example.com"
                            }
                        }
                    },
                    ServiceDescription = new List<DescriptiveText>
                    {
                        new DescriptiveText{ TextContent = "infraservice-2-description-1", TextLanguage = "en" },
                        new DescriptiveText{ TextContent = "infraservice-2-description-2", TextLanguage = "fi" }
                    },
                    ServiceName = new List<DescriptiveText>
                    {
                        new DescriptiveText{ TextContent = "infraservice-2-name-1", TextLanguage = "en" },
                        new DescriptiveText{ TextContent = "infraservice-2-name-2", TextLanguage = "fi" }
                    },
                    ServiceHomepage = new List<Weblink>
                    {
                        new Weblink{ WeblinkURL = "https://infraservice-2-homepage-1.fi", WeblinkLanguage = "en" },
                        new Weblink{ WeblinkURL = "https://infraservice-2-homepage-2.fi", WeblinkLanguage = "fi" }
                    }
                }
            },
            InfraNetwork = new List<InfrastructureNetwork>
            {
                new InfrastructureNetwork
                {
                    InfranetworkRelationType = new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
                    {
                        CodeValue = "relation-typecode-codevalue",
                        CodeDescription = new LanguageVariant
                        {
                            Fi = "relation-typecode-name-fi",
                            En = "relation-typecode-name-en",
                            Sv = "relation-typecode-name-sv"
                        }
                    },
                    RelationValid = true,
                    Pids = new List<PersistentIdentifier>
                    {
                        new PersistentIdentifier
                        {
                            Type = "infranetwork-infra-1-pid1-type",
                            Content = "infranetwork-infra-1-pid1-content"
                        },
                        new PersistentIdentifier
                        {
                            Type = "infranetwork-infra-1-pid2-type",
                            Content = "infranetwork-infra-1-pid2-content"
                        }
                    }
                }
            },
            OrganizationParticipatesInfrastructure = new List<ResearchOrganization>(),
            ResponsibleOrganization = new ResearchOrganization
            {
                DimOrganizationId = 12345,
                OrganizationName = new LanguageVariant
                {
                    Fi = "Responsible organization name FI",
                    En = "Responsible organization name EN",
                    Sv = "Responsible organization name SV"
                }
            }
        };
    }
}