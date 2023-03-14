using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;

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
        entity.BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations = new List<BrDatasetDatasetRelationship>();
        entity.BrDatasetDatasetRelationshipDimResearchDatasets = new List<BrDatasetDatasetRelationship>();

        var model = GetModel();
        model.IncomingDatasetVersionRelations = new List<DatasetRelationBridge>();
        model.OutgoingDatasetRelations = new List<DatasetRelationBridge>();

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
            NameFi = "nameFi",
            NameSv = "nameSv",
            NameEn = "nameEn",
            DescriptionFi = "descFi",
            DescriptionSv = "descSv",
            DescriptionEn = "descEn",
            DatasetCreated = new DateTime(2021, 10, 1),
            FactContributions = new List<FactContribution>
            {
                new()
                {
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
            DimReferencedata = new List<DimReferencedatum>
            {
                new()
                {
                    CodeValue = "languageCode",
                    NameEn = "languageNameEn",
                    NameFi = "languageNameFi",
                    NameSv = "languageNameSv",
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
            DimReferencedataLicenseNavigation = new DimReferencedatum
            {
                CodeValue = "licenseCode",
                NameFi = "licenseNameFi",
                NameSv = "licenseNameSv",
                NameEn = "licenseNameEn",
                CodeScheme = "license"
            },
            DimKeywords = new List<DimKeyword>
            {
                new()
                {
                    Language = "keywordLanguage",
                    Scheme = "keywordScheme",
                    Keyword = "keywordValue"
                }
            },
            BrDatasetDatasetRelationshipDimResearchDatasets = new List<BrDatasetDatasetRelationship>
            {
                new()
                {
                    DimResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = "123",
                        LocalIdentifier = "1"
                    },
                    DimResearchDatasetId2Navigation = new DimResearchDataset
                    {
                        VersionInfo = "123",
                        LocalIdentifier = "2"
                    },
                    DimResearchDatasetId = 32410,
                    DimResearchDatasetId2 = 32410,
                    Type = "version"
                },
                new()
                {
                    DimResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = "321",
                        LocalIdentifier = "3"
                    },
                    DimResearchDatasetId2Navigation = new DimResearchDataset
                    {
                        VersionInfo = "321",
                        LocalIdentifier = "4"
                    },
                    DimResearchDatasetId = 32411,
                    DimResearchDatasetId2 = 32411,
                    Type = "notVersion"
                },
                
            },
            BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations = new List<BrDatasetDatasetRelationship>
            {
                new()
                {
                    DimResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = "321",
                        LocalIdentifier = "5"
                    },
                    DimResearchDatasetId2Navigation = new DimResearchDataset
                    {
                        VersionInfo = "322",
                        LocalIdentifier = "6"
                        
                    },
                    DimResearchDatasetId = 32411,
                    DimResearchDatasetId2 = 32412,
                    Type = "notVersion"
                },
                new()
                {
                    DimResearchDataset = new DimResearchDataset
                    {
                        VersionInfo = "123",
                        LocalIdentifier = "7"
                    },
                    DimResearchDatasetId2Navigation = new DimResearchDataset
                    {
                        VersionInfo = "321",
                        LocalIdentifier = "8"
                    },
                    DimResearchDatasetId = 32410,
                    DimResearchDatasetId2 = 32411,
                    Type = "version"
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
            LocalIdentifier = "localIdentifier",
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
            DatabaseId = Id,
            NameFi = "nameFi",
            NameSv = "nameSv",
            NameEn = "nameEn",
            DescriptionFi = "descFi",
            DescriptionSv = "descSv",
            DescriptionEn = "descEn",
            DatasetCreated = new DateTime(2021, 10, 1),
            Contributors = new List<Contributor>
            {
                new()
                {
                    Organisation = new Organisation
                    {
                        Id = "42",
                        Pids = new PreferredIdentifier[]
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
            FieldOfSciences = new List<ReferenceData>
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
                    Language = "keywordLanguage",
                    Value = "keywordValue"
                }
            },
            PreferredIdentifiers = new List<PreferredIdentifier>
            {
                new()
                {
                    Content = "preferredIdentifierContent",
                    Type = "preferredIdentifierType"
                }
            },
            Identifier = "localIdentifier",
            ResearchDataCatalog = new ResearchDataCatalog
            {
                Id = 7,
                NameFi = "researchDataCatalogNameFi",
                NameSv = "researchDataCatalogNameSv",
                NameEn = "researchDataCatalogNameEn",
                SourceId = "researchDataCatalogSourceId",
                SourceDescription = "researchDataCatalogSourceDescription"
            },
            IncomingDatasetVersionRelations = new List<DatasetRelationBridge>
            {
                new()
                {
                    DatabaseId = 32410, 
                    DatabaseId2 = 32411, 
                    Id = "7", 
                    Id2 = "8", 
                    Type = "version", 
                    VersionNumber = "123", 
                    VersionNumber2 = "321"
                }
            },
            OutgoingDatasetRelations = new List<DatasetRelationBridge>
            {
                new()
                {
                    DatabaseId = 32410, 
                    DatabaseId2 = 32410, 
                    Id = "1", 
                    Id2 = "2", 
                    Type = "version", 
                    VersionNumber = "123", 
                    VersionNumber2 = "123"
                },
                new()
                {
                    DatabaseId = 32411, 
                    DatabaseId2 = 32411, 
                    Id = "3", 
                    Id2 = "4", 
                    Type = "notVersion", 
                    VersionNumber = "321", 
                    VersionNumber2 = "321"
                }
            },
            DatasetRelations = null,
            VersionSet = null,
            IsLatestVersion = null
        };
    }
}
