using System;
using System.Collections.Generic;
using AutoMapper;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;
using Language = CSC.PublicApi.Service.Models.ResearchDataset.Language;
using Version = CSC.PublicApi.Service.Models.ResearchDataset.Version;

namespace CSC.PublicApi.Interface.Tests.Maps;

public class ResearchDatasetProfileTest
{
    private readonly IMapper _mapper;
    private const int Id = 32410;

    public ResearchDatasetProfileTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CSC.PublicApi.Interface.Maps.ResearchDatasetProfile>()).CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_ResearchDatasetServiceModel_ReturnsApiModel()
    {
        // Arrange
        var serviceModel = GetServiceModel();
        var apiModel = GetApiModel();

        // Act
        var result = _mapper.Map<CSC.PublicApi.Interface.Models.ResearchDataset.ResearchDataset>(serviceModel);

        // Assert
        result.Should().BeEquivalentTo(apiModel);
    }

    private static ResearchDataset GetServiceModel()
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

    private static CSC.PublicApi.Interface.Models.ResearchDataset.ResearchDataset GetApiModel()
    {
        return new CSC.PublicApi.Interface.Models.ResearchDataset.ResearchDataset
        {
            NameFi = "nameFi",
            NameSv = "nameSv",
            NameEn = "nameEn",
            DescriptionFi = "descFi",
            DescriptionSv = "descSv",
            DescriptionEn = "descEn",
            DatasetCreated = new DateTime(2021, 10, 1),
            Contributors = new List<CSC.PublicApi.Interface.Models.ResearchDataset.Contributor>
            {
                new()
                {
                    Organisation = new CSC.PublicApi.Interface.Models.ResearchDataset.Organisation
                    {
                        organizationId = "organizationId",
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants"
                    },
                    Person = new CSC.PublicApi.Interface.Models.ResearchDataset.Person {
                        FirstNames  = "personFirstName",
                        LastName = "personLastName",
                        FullName = "personFullName"
                    },
                    Role = new CSC.PublicApi.Interface.Models.ResearchDataset.Role
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldOfSciences = new List<CSC.PublicApi.Interface.Models.ResearchDataset.FieldOfScience>
            {
                new()
                {
                    FieldId = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            Languages = new List<CSC.PublicApi.Interface.Models.ResearchDataset.Language>
            {
                new()
                {
                    Code = "languageCode",
                    NameEn = "languageNameEn",
                    NameFi = "languageNameFi",
                    NameSv = "languageNameSv"
                }
            },
            AccessType = new CSC.PublicApi.Interface.Models.ResearchDataset.Availability
            {
                Code = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn"
            },
            License = new CSC.PublicApi.Interface.Models.ResearchDataset.License
            {
                Code = "licenseCode",
                NameFi = "licenseNameFi",
                NameSv = "licenseNameSv",
                NameEn = "licenseNameEn",
            },
            Keywords = new List<CSC.PublicApi.Interface.Models.ResearchDataset.Keyword>
            {
                new()
                {
                    Language = "keywordLanguage",
                    Scheme = "keywordScheme",
                    Value = "keywordValue"
                }
            },
            DatasetRelations = new List<CSC.PublicApi.Interface.Models.ResearchDataset.DatasetRelation>
            {
                new()
                {
                    Id = "32410",
                    Type = "dataSetRelationType"
                }
            },
            PreferredIdentifiers = new List<CSC.PublicApi.Interface.Models.ResearchDataset.PreferredIdentifier>
            {
                new()
                {
                    Content = "preferredIdentifierContent",
                    Type = "preferredIdentifierType"
                }
            },
            LocalIdentifier = "localIdentifier",
            FairDataUrl = "https://etsin.fairdata.fi/dataset/localIdentifier",
            ResearchDataCatalog = new CSC.PublicApi.Interface.Models.ResearchDataset.ResearchDataCatalog
            {
                Id = 7,
                NameFi = "researchDataCatalogNameFi",
                NameSv = "researchDataCatalogNameSv",
                NameEn = "researchDataCatalogNameEn",
                SourceId = "researchDataCatalogSourceId",
                SourceDescription = "researchDataCatalogSourceDescription"
            },
            VersionSet = new List<CSC.PublicApi.Interface.Models.ResearchDataset.Version>
            {
                new()
                {
                    Identifier = "32410",
                    VersionNumber = "123"
                },
                new()
                {
                    Identifier = "32411",
                    VersionNumber = "321"
                }
            },
            IsLatestVersion = false
        };
    }
}
