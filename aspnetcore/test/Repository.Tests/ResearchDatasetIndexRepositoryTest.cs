using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;

namespace Repository.Tests;

public class ResearchDatasetIndexRepositoryTest
{
    private readonly ResearchDatasetIndexRepository _researchDatasetIndexRepository;
    
    public ResearchDatasetIndexRepositoryTest()
    {
        _researchDatasetIndexRepository = new ResearchDatasetIndexRepository(null, null);
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
        researchDataset.FieldOfSciences.Should().BeNull();
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
            DatasetCreated = new DateTime(2021, 10, 1),
            Contributors = new List<Contributor>
            {
                new()
                {
                    Organisation = new Organisation
                    {
                        Id = "-1",
                        Pids = Array.Empty<PreferredIdentifier>(),
                        NameFi = " ",
                        NameSv = " ",
                        NameEn = " ",
                        NameVariants = " "
                    },
                    Person = null,
                    Role = new Role
                    {
                        NameFi = "roleNameFi",
                        NameSv = "roleNameSv",
                        NameEn = "roleNameEn"
                    }
                }
            },
            FieldOfSciences = new List<FieldOfScience>(),
            Languages = new List<Language>(),
            AccessType = new Availability
            {
                Code = "accessTypeCode",
                NameFi = "accessTypeNameFi",
                NameSv = "accessTypeNameSv",
                NameEn = "accessTypeNameEn"
            },
            License = new License
            {
                Code = null,
                NameFi = null,
                NameSv = null,
                NameEn = null,
            },
            Keywords = new List<Keyword>(),
            PreferredIdentifiers = new List<PreferredIdentifier>(),
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
        };
    }
}