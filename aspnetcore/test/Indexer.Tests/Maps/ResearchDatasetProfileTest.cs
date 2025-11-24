using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;
using Organization = CSC.PublicApi.Service.Models.ResearchDataset.Organization;
using Version = CSC.PublicApi.Service.Models.ResearchDataset.Version;

namespace CSC.PublicApi.Indexer.Tests.Maps;

public class ResearchDatasetProfileTest
{
    private readonly IMapper _mapper;
    private const int Id = 32410;

    public ResearchDatasetProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ResearchDatasetProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void ProjectTo_DimResearchDataset_ShouldBeMappedToResearchDataset()
    {
        // Arrange
        var entity = GetEntity();
        var model = GetModel();

        // Act
        var result = Act_Map(entity);

        // Assert
        result.Should().BeEquivalentTo(model, options => options);
    }

    [Fact]
    public void ProjectTo_DimResearchDatasetWithoutVersions_ShouldBeMappedToResearchDataset()
    {
        // Arrange
        var entity = GetEntity();
        // Relation type "HasVersion" is not included in related datasets.
        entity.FactRelationFromResearchDatasets = new List<FactRelation>
        {
            new FactRelation()
            {
                FromResearchDataset = new DimResearchDataset
                {
                    VersionInfo = 1,
                    LocalIdentifier = "localIdentifier_1"
                },
                ToResearchDataset = new DimResearchDataset
                {
                    VersionInfo = 3,
                    LocalIdentifier = "localIdentifier_3"
                },
                FromResearchDatasetId = 111,
                ToResearchDatasetId = 113,
                RelationTypeCodeNavigation = new DimReferencedatum
                {
                    CodeValue = "IsPartOf"
                }
            }
        };

        var model = GetModel();
        model.DatasetRelations = new List<DatasetRelation>()
        {
            new DatasetRelation()
            {
                Id = "localIdentifier_3",
                Type = "IsPartOf"
            }
        };
        model.VersionSet = new List<Version>();

        // Act
        var result = Act_Map(entity);

        // Assert
        result.Should().BeEquivalentTo(model, options => options);
    }

    private ResearchDataset Act_Map(DimResearchDataset dbEntity)
    {
        var entityQueryable = new List<DimResearchDataset>
        {
            dbEntity
        }.AsQueryable();

        var modelCollection = _mapper.ProjectTo<ResearchDataset>(entityQueryable);

        return modelCollection.Single();
    }

    private static DimResearchDataset GetEntity()
    {
        return new DimResearchDataset
        {
            Id = Id,
            DimDescriptiveItems = new List<DimDescriptiveItem>
            {
                new()
                {
                    DescriptiveItemType = "name",
                    DescriptiveItemLanguage = "fi",
                    DescriptiveItem = "nameFi"
                },
                new()
                {
                    DescriptiveItemType = "name",
                    DescriptiveItemLanguage = "sv",
                    DescriptiveItem = "nameSv"
                },
                new()
                {
                    DescriptiveItemType = "name",
                    DescriptiveItemLanguage = "en",
                    DescriptiveItem = "nameEn"
                },
                new()
                {
                    DescriptiveItemType = "description",
                    DescriptiveItemLanguage = "fi",
                    DescriptiveItem = "descFi"
                },
                new()
                {
                    DescriptiveItemType = "description",
                    DescriptiveItemLanguage = "sv",
                    DescriptiveItem = "descSv"
                },
                new()
                {
                    DescriptiveItemType = "description",
                    DescriptiveItemLanguage = "en",
                    DescriptiveItem = "descEn"
                }
            },
            DatasetCreated = new DateTime(2021, 10, 1),
            FactContributions = new List<FactContribution>
            {
                new()
                {
                    DimOrganizationId = 42,
                    DimOrganization = new DimOrganization
                    {
                        Id = 42,
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants",
                        DimPids = new List<DimPid>
                        {
                            new()
                            {
                                PidContent = "pidContent1",
                                PidType = "BusinessID"
                            },
                            new()
                            {
                                PidContent = "pidContent2",
                                PidType = "PIC"
                            },
                            new()
                            {
                                PidContent = "pidContent3",
                                PidType = "WrongType"
                            },
                        }
                    },
                    DimName = new DimName
                    {
                        FirstNames  = "personFirstName",
                        LastName = "personLastName",
                        FullName = "personFullName"
                    },
                    DimReferencedataActorRole = new DimReferencedatum
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    },
                    DimIdentifierlessDataId = 99,
                    DimIdentifierlessData = new DimIdentifierlessDatum
                    {
                        Id = 99,
                        ValueFi = "identifierlessDataValueFi",
                        ValueSv = "identifierlessDataValueSv",
                        ValueEn = "identifierlessDataValueEn",
                        ValueUnd = "identifierlessDataValueUnd",
                        DimOrganizationId = 43,
                        DimOrganization = new DimOrganization
                        {
                            Id = 43,
                            NameFi = "identifierlessOrganizationNameFi",
                            NameSv = "identifierlessOrganizationNameSv",
                            NameEn = "identifierlessOrganizationNameEn",
                            NameVariants = "identifierlessOrganizationNameVariants",
                            DimPids = new List<DimPid>
                            {
                                new()
                                {
                                    PidContent = "pidContent4",
                                    PidType = "BusinessID"
                                },
                                new()
                                {
                                    PidContent = "pidContent5",
                                    PidType = "PIC"
                                },
                                new()
                                {
                                    PidContent = "pidContent6",
                                    PidType = "WrongType"
                                }
                            }
                        }
                    }
                }
            },
            FactDimReferencedataFieldOfSciences = new List<FactDimReferencedataFieldOfScience>
            {
                new()
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeValue = "fieldOfScienceFieldId",
                        NameFi = "fieldOfScienceNameFi",
                        NameSv = "fieldOfScienceNameSv",
                        NameEn = "fieldOfScienceNameEn"
                    }
                }
            },
            FactReferencedata = new List<FactReferencedatum>
            {
                new()
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeValue = "languageCode",
                        NameEn = "languageNameEn",
                        NameFi = "languageNameFi",
                        NameSv = "languageNameSv",
                        CodeScheme = "languages"
                    }
                },
                new()
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeValue = "licenseCode",
                        NameFi = "licenseNameFi",
                        NameSv = "licenseNameSv",
                        NameEn = "licenseNameEn",
                        CodeScheme = "license"
                    }
                },
                new()
                {
                    DimReferencedata = new DimReferencedatum
                    {
                        CodeValue = "accessTypeCode",
                        NameFi = "accessTypeNameFi",
                        NameSv = "accessTypeNameSv",
                        NameEn = "accessTypeNameEn",
                        CodeScheme = "access_type"
                    }
                }
            },
            DimReferencedataAvailabilityNavigation = new DimReferencedatum
            {
                CodeValue = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn",
                CodeScheme = "access_type"
            },
            FactKeywords = new List<FactKeyword>
            {
                new()
                {
                    DimKeyword = new DimKeyword
                    {
                        Keyword = "keyword1",
                        Language = "keywordLanguage1",
                        Scheme = "Avainsana"
                    }
                },
                new()
                {
                    DimKeyword = new DimKeyword
                    {
                        Keyword = "subjectHeading1",
                        Language = "keywordLanguage2",
                        Scheme = "Theme"
                    }
                },
                new()
                {
                    DimKeyword = new DimKeyword
                    {
                        Keyword = "wrongSchemeKeyword",
                        Language = "keywordLanguage3",
                        Scheme = "WrongScheme"
                    }
                }
            },
            VersionInfo = 1,
            FactRelationFromResearchDatasets = new List<FactRelation>
            {
                new()
                {
                    FromResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = 1,
                        LocalIdentifier = "localIdentifier_1"
                    },
                    ToResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = 1,
                        LocalIdentifier = "localIdentifier_1"
                    },
                    FromResearchDatasetId = 111,
                    ToResearchDatasetId = 111,
                    RelationTypeCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "HasVersion"
                    }
                },
                new()
                {
                    FromResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = 1,
                        LocalIdentifier = "localIdentifier_1"
                    },
                    ToResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = 2,
                        LocalIdentifier = "localIdentifier_2"
                    },
                    FromResearchDatasetId = 111,
                    ToResearchDatasetId = 112,
                    RelationTypeCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "HasVersion"
                    }
                },
                new()
                {
                    FromResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = 1,
                        LocalIdentifier = "localIdentifier_1"
                    },
                    ToResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = 3,
                        LocalIdentifier = "localIdentifier_3"
                    },
                    FromResearchDatasetId = 111,
                    ToResearchDatasetId = 113,
                    RelationTypeCodeNavigation = new DimReferencedatum
                    {
                        CodeValue = "IsPartOf"
                    }
                }
            },
            DimPids = new List<DimPid>
            {
                new()
                {
                    PidContent = "preferredIdentifierContent",
                    PidType = "preferredIdentifierType"
                }
            },
            LocalIdentifier = "ab6730f7-a2a2-407b-9e18-d03b650d011f",
            DimResearchDataCatalog = new DimResearchDataCatalog
            {
                Id = 7,
                NameFi = "researchDataCatalogNameFi",
                NameSv = "researchDataCatalogNameSv",
                NameEn = "researchDataCatalogNameEn",
                SourceId = "researchDataCatalogSourceId",
                SourceDescription = "researchDataCatalogSourceDescription"
            }
        };
    }

    private static ResearchDataset GetModel()
    {
        return new ResearchDataset
        {
            ExportSortId = 32410,
            DatabaseId = Id,
            NameFi = "nameFi",
            NameSv = "nameSv",
            NameEn = "nameEn",
            DescriptionFi = "descFi",
            DescriptionSv = "descSv",
            DescriptionEn = "descEn",
            Created = new DateTime(2021, 10, 1),
            ContributorsHelper = new List<ContributorHelper>
            {
                new()
                {
                    FactContribution_DimOrganizationId = 42,
                    FactContribution_DimIdentifierlessDataId = 99,
                    FactContribution_DimIdentifierlessData_DimOrganizationId = 43,
                    Organization_From_FactContribution_DimOrganization = new Organization
                    {
                        Id = "42",
                        Pids = new List<PersistentIdentifier>
                        {
                            new()
                            {
                                Content = "pidContent1",
                                Type = "BusinessID"
                            },
                            new()
                            {
                            Content = "pidContent2",
                            Type = "PIC"
                            }
                        },
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants"
                    },
                    Organization_From_FactContribution_DimIdentifierlessData = new Organization
                    {
                        Id = null,
                        NameEn = "identifierlessDataValueEn",
                        NameFi = "identifierlessDataValueFi",
                        NameSv = "identifierlessDataValueSv",
                        NameVariants = "identifierlessDataValueUnd",
                        Pids = null
                    },
                    Organization_From_FactContribution_DimIdentifierlessData_DimOrganization = new Organization
                    {
                        Id = "43",
                        NameEn = "identifierlessOrganizationNameEn",
                        NameFi = "identifierlessOrganizationNameFi",
                        NameSv = "identifierlessOrganizationNameSv",
                        NameVariants = "identifierlessOrganizationNameVariants",
                        Pids = new List<PersistentIdentifier>
                        {
                            new()
                            {
                                Content = "pidContent4",
                                Type = "BusinessID"
                            },
                            new()
                            {
                            Content = "pidContent5",
                            Type = "PIC"
                            }
                        },
                    },
                    Person = new Person
                    {
                        Name = "personFullName"
                    },
                    Role = new ReferenceData
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldsOfScience = new List<ReferenceData>
            {
                new()
                {
                    Code = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            Languages = new List<ReferenceData>
            {
                new()
                {
                    Code = "languageCode",
                    NameEn = "languageNameEn",
                    NameFi = "languageNameFi",
                    NameSv = "languageNameSv"
                }
            },
            AccessType = new ReferenceData
            {
                Code = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn"
            },
            License = new ReferenceData
            {
                Code = "licenseCode",
                NameFi = "licenseNameFi",
                NameSv = "licenseNameSv",
                NameEn = "licenseNameEn",
            },
            Keywords = new List<Keyword>
            {
                new()
                {
                    Value = "keyword1",
                    Language = "keywordLanguage1",
                    Scheme = "Avainsana"
                }
            },
            SubjectHeadings = new List<Keyword>
            {
                new()
                {
                    Value = "subjectHeading1",
                    Language = "keywordLanguage2",
                    Scheme = "Theme"
                }
            },
            PersistentIdentifiers = new List<PersistentIdentifier>
            {
                new()
                {
                    Content = "preferredIdentifierContent",
                    Type = "preferredIdentifierType"
                }
            },
            Id = "ab6730f7-a2a2-407b-9e18-d03b650d011f",
            ResearchDataCatalog = new ResearchDataCatalog
            {
                Id = 7,
                NameFi = "researchDataCatalogNameFi",
                NameSv = "researchDataCatalogNameSv",
                NameEn = "researchDataCatalogNameEn",
                SourceId = "researchDataCatalogSourceId",
                SourceDescription = "researchDataCatalogSourceDescription"
            },
            DatasetRelations = new List<DatasetRelation>
            {
                new()
                {
                    Id = "localIdentifier_3",
                    Type = "IsPartOf"
                }
            },
            VersionInfo = 1,
            VersionSet = new List<Version>
            {
                new()
                {
                    Identifier = "localIdentifier_1",
                    VersionNumber = 1
                },
                new()
                {
                    Identifier = "localIdentifier_2",
                    VersionNumber = 2,
                }
            },
            IsLatestVersion = null
        };
    }
}