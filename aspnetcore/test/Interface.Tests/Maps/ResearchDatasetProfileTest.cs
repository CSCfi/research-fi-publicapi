using System;
using System.Collections.Generic;
using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using ResearchFi;
using ResearchFi.CodeList;
using ResearchFi.Query;
using Xunit;
using Keyword = CSC.PublicApi.Service.Models.Keyword;
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
        var result = _mapper.Map<ResearchFi.ResearchDataset.ResearchDataset>(serviceModel);

        // Assert
        result.Should().BeEquivalentTo(apiModel);
    }
    
    [Fact]
    public void Map_GetResearchDatasetsQueryParameters_ConvertsListsToArrays()
    {
        // Arrange
        var apiModel = new GetResearchDatasetsQueryParameters
        {
            Access = "access",
            Description = "description",
            FieldOfScience = "111, 112, 113",
            Keywords = "keywords",
            Language = "fin, eng, est",
            License = "license",
            Name = "name",
            DateFrom = DateTime.MinValue,
            DateTo = DateTime.MaxValue,
            OrganizationId = "organisationId",
            OrganizationName = "organisationName",
            PersonName = "personName",
            IsLatestVersion = true,
            RelatedDatasetId = "relatedDatasetId",
            ResearchDataCatalog = "researchDataCatalog"

        };
        var elasticModel = new ResearchDatasetSearchParameters
        {
            Access = "access",
            Description = "description",
            FieldOfScience = new List<string> { "111", "112", "113" },
            Keywords = "keywords",
            Language = new List<string> { "fin", "eng", "est" },
            License = "license",
            Name = "name",
            DateFrom = DateTime.MinValue,
            DateTo = DateTime.MaxValue,
            OrganizationId = "organisationId",
            OrganizationName = "organisationName",
            PersonName = "personName",
            IsLatestVersion = true,
            RelatedDatasetId = "relatedDatasetId",
            ResearchDataCatalog = "researchDataCatalog"
        };

        // Act
        var result = _mapper.Map<ResearchDatasetSearchParameters>(apiModel);

        // Assert
        result.Should().BeEquivalentTo(elasticModel);
    }
    
    private static ResearchDataset GetServiceModel()
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
            Created = new DateTime(2021, 10, 1),
            Contributors = new List<Contributor>
            {
                new()
                {
                    Organization = new Organization
                    {
                        Id = "organizationId",
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants"
                    },
                    Person = new Person {
                        Name = "personFullName"
                    },
                    Role = new Service.Models.ReferenceData
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldsOfScience = new List<Service.Models.ReferenceData>
            {
                new()
                {
                    Code = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            Languages = new List<Service.Models.ReferenceData>
            {
                new()
                {
                    Code = "languageCode",
                    NameEn = "languageNameEn",
                    NameFi = "languageNameFi",
                    NameSv = "languageNameSv"
                }
            },
            AccessType = new Service.Models.ReferenceData
            {
                Code = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn"
            },
            License = new Service.Models.ReferenceData
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
            DatasetRelations = new List<DatasetRelation>
            {
                new()
                {
                    Id = "32410",
                    Type = "dataSetRelationType"
                }
            },
            PersistentIdentifiers = new List<Service.Models.PersistentIdentifier>
            {
                new()
                {
                    Content = "preferredIdentifierContent",
                    Type = "preferredIdentifierType"
                }
            },
            Id = "localIdentifier",
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
                    Identifier = "45670894-2a22-422e-8858-2fe52938276d",
                    VersionNumber = "123"
                },
                new()
                {
                    Identifier = "9936fbc2-f043-4c27-9a65-a52472aa94cb",
                    VersionNumber = "321"
                }
            },
            IsLatestVersion = false
        };
    }

    private static ResearchFi.ResearchDataset.ResearchDataset GetApiModel()
    {
        return new ResearchFi.ResearchDataset.ResearchDataset
        {
            NameFi = "nameFi",
            NameSv = "nameSv",
            NameEn = "nameEn",
            DescriptionFi = "descFi",
            DescriptionSv = "descSv",
            DescriptionEn = "descEn",
            Created = new DateTime(2021, 10, 1),
            Contributors = new List<ResearchFi.ResearchDataset.Contributor>
            {
                new()
                {
                    Organization = new ResearchFi.ResearchDataset.Organization
                    {
                        Id = "organizationId",
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants"
                    },
                    Person = new ResearchFi.ResearchDataset.Person {
                        Name = "personFullName"
                    },
                    Role = new AgentRole
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldsOfScience = new List<FieldOfScience>
            {
                new()
                {
                    Code = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            Languages = new List<LexvoLanguage>
            {
                new()
                {
                    Code = "languageCode",
                    NameEn = "languageNameEn",
                    NameFi = "languageNameFi",
                    NameSv = "languageNameSv"
                }
            },
            AccessType = new AccessType
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
            Keywords = new List<ResearchFi.Keyword>
            {
                new()
                {
                    Language = "keywordLanguage",
                    Value = "keywordValue"
                }
            },
            DatasetRelations = new List<ResearchFi.ResearchDataset.DatasetRelation>
            {
                new()
                {
                    Id = "32410",
                    Type = "dataSetRelationType"
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
            Id = "localIdentifier",
            FairDataUrl = "https://etsin.fairdata.fi/dataset/localIdentifier",
            ResearchDataCatalog = new ResearchFi.ResearchDataset.ResearchDataCatalog
            {
                Id = 7,
                NameFi = "researchDataCatalogNameFi",
                NameSv = "researchDataCatalogNameSv",
                NameEn = "researchDataCatalogNameEn",
                SourceId = "researchDataCatalogSourceId",
                SourceDescription = "researchDataCatalogSourceDescription"
            },
            VersionSet = new List<ResearchFi.ResearchDataset.Version>
            {
                new()
                {
                    Identifier = "45670894-2a22-422e-8858-2fe52938276d",
                    VersionNumber = "123"
                },
                new()
                {
                    Identifier = "9936fbc2-f043-4c27-9a65-a52472aa94cb",
                    VersionNumber = "321"
                }
            },
            IsLatestVersion = false
        };
    }
}
