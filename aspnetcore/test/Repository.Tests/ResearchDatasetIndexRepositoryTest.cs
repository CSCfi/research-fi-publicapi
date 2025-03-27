using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;

namespace Repository.Tests;

public class ResearchDatasetIndexRepositoryTest
{
    private readonly ResearchDatasetIndexRepository _researchDatasetIndexRepository;
    
    public ResearchDatasetIndexRepositoryTest()
    {
        _researchDatasetIndexRepository = new ResearchDatasetIndexRepository(null, null, null);
    }
    
    [Fact]
    public void PerformInMemoryOperations_ResearchDataset_IsHandledCorrectly()
    {
        // Arrange
        var researchDatasets = new List<object>
        {
            GetModel()
        };
        
        // Act
        var resultDatasets = _researchDatasetIndexRepository.PerformInMemoryOperations(researchDatasets);
        
        // Assert
        resultDatasets.Should().NotBeNullOrEmpty();
        var researchDatasetObject = researchDatasets.FirstOrDefault();
        researchDatasetObject.Should().NotBeNull().And.BeAssignableTo<ResearchDataset>();
        var researchDataset = (ResearchDataset)researchDatasetObject!;
        
        researchDataset.DatasetRelations.Should().NotBeNullOrEmpty();
        var datasetRelation = researchDataset.DatasetRelations!.FirstOrDefault();
        datasetRelation.Should().NotBeNull();
        datasetRelation!.Id.Should().Be("4");
        datasetRelation.Type.Should().Be("notVersion");
        
        researchDataset.VersionSet.Should().NotBeNullOrEmpty();
        researchDataset.VersionSet.Should().HaveCount(2);
        researchDataset.VersionSet.Should().ContainSingle(v => v.Identifier == "1" && v.VersionNumber == "1");
        researchDataset.VersionSet.Should().ContainSingle(v => v.Identifier == "2" && v.VersionNumber == "2");

        researchDataset.IsLatestVersion.Should().Be(false);
        researchDataset.Contributors.Should().BeNull();
        researchDataset.FieldsOfScience.Should().BeNull();
        researchDataset.Keywords.Should().BeNull();
        researchDataset.Languages.Should().BeNull();
        researchDataset.License.Should().BeNull();
    }
    
    [Fact]
    public void PerformInMemoryOperations_ResearchDatasetWithNoVersions_IsLatestVersion()
    {
        // Arrange
        var researchDataset = GetModel();
        researchDataset.IncomingDatasetVersionRelations = null;
        researchDataset.OutgoingDatasetRelations = null;
        var researchDatasets = new List<object>
        {
            researchDataset
        };
        
        // Act
        var resultDatasets = _researchDatasetIndexRepository.PerformInMemoryOperations(researchDatasets);
        
        // Assert
        resultDatasets.Should().NotBeNullOrEmpty();
        var researchDatasetObject = researchDatasets.FirstOrDefault();
        researchDatasetObject.Should().NotBeNull().And.BeAssignableTo<ResearchDataset>();
        var resultResearchDataset = (ResearchDataset)researchDatasetObject!;

        resultResearchDataset.DatasetRelations.Should().BeNull();
        resultResearchDataset.VersionSet.Should().BeNull();
        resultResearchDataset.IsLatestVersion.Should().Be(true);
    }
    
    [Fact]
    public void PerformInMemoryOperations_ResearchDatasetWithLatestVersion_IsLatestVersion()
    {
        // Arrange
        var researchDataset = GetModel();
        researchDataset.DatabaseId = 101;
        var researchDatasets = new List<object>
        {
            researchDataset
        };
        
        // Act
        var resultDatasets = _researchDatasetIndexRepository.PerformInMemoryOperations(researchDatasets);
        
        // Assert
        resultDatasets.Should().NotBeNullOrEmpty();
        var researchDatasetObject = researchDatasets.FirstOrDefault();
        researchDatasetObject.Should().NotBeNull().And.BeAssignableTo<ResearchDataset>();
        var resultResearchDataset = (ResearchDataset)researchDatasetObject!;

        resultResearchDataset.VersionSet.Should().NotBeNull();
        resultResearchDataset.IsLatestVersion.Should().Be(true);

        // Check that ResearchfiUrl is set correctly in HandleResearchfiUrl
        resultResearchDataset.ResearchfiUrl.Should().NotBeNull();
        resultResearchDataset.ResearchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/dataset/localIdentifier");
        resultResearchDataset.ResearchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/dataset/localIdentifier");
        resultResearchDataset.ResearchfiUrl.En.Should().Be("https://research.fi/en/results/dataset/localIdentifier");
    }
    
    private static ResearchDataset GetModel()
    {
        return new ResearchDataset
        {
            DatabaseId = 100,
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
                        Id = "-1",
                        Pids = new List<PersistentIdentifier>(),
                        NameFi = " ",
                        NameSv = " ",
                        NameEn = " ",
                        NameVariants = " "
                    },
                    Person = null,
                    Role = new ReferenceData
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldsOfScience = new List<ReferenceData>(),
            Languages = new List<ReferenceData>(),
            AccessType = new ReferenceData()
            {
                Code = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn"
            },
            License = new ReferenceData()
            {
                Code = null,
                NameFi = null,
                NameSv = null,
                NameEn = null,
            },
            Keywords = new List<Keyword>(),
            PersistentIdentifiers = new List<PersistentIdentifier>(),
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
            IncomingDatasetVersionRelations = new List<DatasetRelationBridge>
            {
                new()
                {
                    DatabaseId = 101, 
                    DatabaseId2 = 100, 
                    Id = "2", 
                    Id2 = "1", 
                    Type = "version", 
                    VersionNumber = "2", 
                    VersionNumber2 = "1"
                }
            },
            OutgoingDatasetRelations = new List<DatasetRelationBridge>
            {
                new()
                {
                    DatabaseId = 100,
                    DatabaseId2 = 100,
                    Id = "1", 
                    Id2 = "1", 
                    Type = "version", 
                    VersionNumber = "1", 
                    VersionNumber2 = "1"
                },
                new()
                {
                    DatabaseId = 100,
                    DatabaseId2 = 101,
                    Id = "1", 
                    Id2 = "2", 
                    Type = "version", 
                    VersionNumber = "1", 
                    VersionNumber2 = "2"
                },
                new()
                {
                    DatabaseId = 100, 
                    DatabaseId2 = 105, 
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
            // ResearchfiUrl should be set in HandleResearchfiUrl
        };
    }
}