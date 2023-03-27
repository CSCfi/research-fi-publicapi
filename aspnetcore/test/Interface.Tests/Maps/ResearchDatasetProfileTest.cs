using System;
using System.Collections.Generic;
using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;
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
            OrganisationId = "organisationId",
            OrganisationName = "organisationName",
            PersonName = "personName",
            IsLatestVersion = true,
            RelatedDatasetId = "relatedDatasetId",
            ResearchDataCatalog = "researchDataCatalog"

        };
        var elasticModel = new ResearchDatasetSearchParameters
        {
            Access = "access",
            Description = "description",
            FieldOfScience = new[] { "111", "112", "113" },
            Keywords = "keywords",
            Language = new[] { "fin", "eng", "est" },
            License = "license",
            Name = "name",
            DateFrom = DateTime.MinValue,
            DateTo = DateTime.MaxValue,
            OrganisationId = "organisationId",
            OrganisationName = "organisationName",
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
            DatasetCreated = new DateTime(2021, 10, 1),
            Contributors = new List<Contributor>
            {
                new()
                {
                    Organisation = new Organisation
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
            FieldOfSciences = new List<Service.Models.ReferenceData>
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
                        Id = "organizationId",
                        NameFi = "organizationNameFi",
                        NameSv = "organizationNameSv",
                        NameEn = "organizationNameEn",
                        NameVariants = "organizationNameVariants"
                    },
                    Person = new CSC.PublicApi.Interface.Models.ResearchDataset.Person {
                        Name = "personFullName"
                    },
                    Role = new Models.ReferenceData
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldOfSciences = new List<Models.ReferenceData>
            {
                new()
                {
                    Code = "fieldOfScienceFieldId",
                    NameFi = "fieldOfScienceNameFi",
                    NameSv = "fieldOfScienceNameSv",
                    NameEn = "fieldOfScienceNameEn"
                }
            },
            Languages = new List<Models.ReferenceData>
            {
                new()
                {
                    Code = "languageCode",
                    NameEn = "languageNameEn",
                    NameFi = "languageNameFi",
                    NameSv = "languageNameSv"
                }
            },
            AccessType = new Models.ReferenceData
            {
                Code = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn"
            },
            License = new Models.ReferenceData
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
            Identifier = "localIdentifier",
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
