using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Repositories.Maps;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;
using Language = CSC.PublicApi.Service.Models.ResearchDataset.Language;
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
    public void ProjectTo_DimRearchDataset_ShouldBeMappedToResearchDataset()
    {
        // Arrange
        var entity = GetEntity();
        var model = GetModel();

        // Act
        var result = Act_Map(entity);

        // Assert
        result.Should().BeEquivalentTo(model, options => options.Excluding(su => su.IncomingVersions).Excluding(su => su.OutgoingVersions));
    }

    [Fact]
    public void ProjectTo_DimRearchDatasetWithoutVersions_ShouldBeMappedToResearchDataset()
    {
        // Arrange
        var entity = GetEntity();
        entity.BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations = new List<BrDatasetDatasetRelationship>();
        entity.BrDatasetDatasetRelationshipDimResearchDatasets = new List<BrDatasetDatasetRelationship>();

        var model = GetModel();
        model.VersionSet = new List<Version>();
        model.DatasetRelations = new List<DatasetRelation>();
        model.IsLatestVersion = true;

        // Act
        var result = Act_Map(entity);

        // Assert
        result.Should().BeEquivalentTo(model, options => options.Excluding(su => su.IncomingVersions).Excluding(su => su.OutgoingVersions));
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
            FactContributions = new List<FactContribution>()
            {
                new()
                {
                    DimOrganization = new DimOrganization
                    {
                        OrganizationId = "organizationId",
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants"
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
            DimFieldOfSciences = new List<DimFieldOfScience>()
            {
                new()
                {
                    FieldId = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            DimReferencedata = new List<DimReferencedatum>()
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
            DimKeywords = new List<DimKeyword>()
            {
                new()
                {
                    Language = "keywordLanguage",
                    Scheme = "keywordScheme",
                    Keyword = "keywordValue"
                }
            },
            BrDatasetDatasetRelationshipDimResearchDatasets = new List<BrDatasetDatasetRelationship>()
            {
                new()
                {
                    DimResearchDatasetId2Navigation = new DimResearchDataset
                    {
                        VersionInfo = "123"
                    },
                    DimResearchDataset = new DimResearchDataset()
                    {
                        VersionInfo = "123"
                    },
                    DimResearchDatasetId = 32410,
                    DimResearchDatasetId2 = 32410,
                    Type = "dataSetRelationType"
                }
            },
            BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations = new List<BrDatasetDatasetRelationship>()
            {
                new()
                {
                    DimResearchDatasetId2Navigation = new DimResearchDataset
                    {
                        VersionInfo = "321"
                    },
                    DimResearchDataset = new DimResearchDataset()
                    {
                        VersionInfo = "321"
                    },
                    DimResearchDatasetId = 32411,
                    DimResearchDatasetId2 = 32411,
                    Type = "dataSetRelationType"
                },
                new()
                {
                    DimResearchDatasetId2Navigation = new DimResearchDataset
                    {
                        VersionInfo = "321"
                    },
                    DimResearchDataset = new DimResearchDataset()
                    {
                        VersionInfo = "123"
                    },
                    DimResearchDatasetId = 32410,
                    DimResearchDatasetId2 = 32411,
                    Type = "dataSetRelationType"
                }
            },
            DimPids = new List<DimPid>()
            {
                new()
                {
                    PidContent = "preferredIdentifierContent",
                    PidType = "preferredIdentifierType"
                }
            },
            LocalIdentifier = "localIdentifier",
            DimResearchDataCatalog = new DimResearchDataCatalog()
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
            Id = Id,
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
                        organizationId = "organizationId",
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants"
                    },
                    Person = new Person {
                        FirstNames  = "personFirstName",
                        LastName = "personLastName",
                        FullName = "personFullName"
                    },
                    Role = new Role
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldOfSciences = new List<FieldOfScience>
            {
                new()
                {
                    FieldId = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            Languages = new List<Language>
            {
                new()
                {
                    Code = "languageCode",
                    NameEn = "languageNameEn",
                    NameFi = "languageNameFi",
                    NameSv = "languageNameSv"
                }
            },
            AccessType = new Availability
            {
                Code = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn"
            },
            License = new License
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
                    Scheme = "keywordScheme",
                    Value = "keywordValue"
                }
            },
            DatasetRelations = new List<DatasetRelation>
            {
                new()
                {
                    Id = "32410",
                    Type = "dataSetRelationType"
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
            LocalIdentifier = "localIdentifier",
            FairDataUrl = "https://etsin.fairdata.fi/dataset/localIdentifier",
            ResearchDataCatalog = new ResearchDataCatalog
            {
                Id = 7,
                NameFi = "researchDataCatalogNameFi",
                NameSv = "researchDataCatalogNameSv",
                NameEn = "researchDataCatalogNameEn",
                SourceId = "researchDataCatalogSourceId",
                SourceDescription = "researchDataCatalogSourceDescription"
            },
            VersionSet = new List<Version>
            {
                new()
                {
                    Identifier = 32410,
                    VersionNumber = "123"
                },
                new()
                {
                    Identifier = 32411,
                    VersionNumber = "321"
                }
            },
            IsLatestVersion = false
        };
    }
}
