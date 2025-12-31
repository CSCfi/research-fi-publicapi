using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using FluentAssertions;
using Xunit;
using Version = CSC.PublicApi.Service.Models.ResearchDataset.Version;

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

        researchDataset.IsLatestVersion.Should().Be(false, "IsLatestVersion should be false");
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
        researchDataset.VersionSet = null; // No versions
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
        resultResearchDataset.IsLatestVersion.Should().Be(true, "IsLatestVersion should be true");
    }
    
    [Fact]
    public void PerformInMemoryOperations_ResearchDatasetWithLatestVersion_IsLatestVersion()
    {
        // Arrange
        var researchDataset = GetModel();
        researchDataset.VersionInfo = 3; // Set to higher value than any in VersionSet
        researchDataset.VersionSet = new List<Version>()
        {
            new Version
            {
                Identifier = "localIdentifier-1",
                VersionNumber = 1
            },
            new Version
            {
                Identifier = "localIdentifier-1",
                VersionNumber = 2
            }
        };
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
        resultResearchDataset.ResearchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/dataset/localIdentifier-1");
        resultResearchDataset.ResearchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/dataset/localIdentifier-1");
        resultResearchDataset.ResearchfiUrl.En.Should().Be("https://research.fi/en/results/dataset/localIdentifier-1");
    }
    
    private static ResearchDataset GetModel()
    {
        return new ResearchDataset
        {
            DatabaseId = 100,
            Id = "localIdentifier-1",
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
            ResearchDataCatalog = new ResearchDataCatalog
            {
                Id = 7,
                NameFi = "researchDataCatalogNameFi",
                NameSv = "researchDataCatalogNameSv",
                NameEn = "researchDataCatalogNameEn",
                SourceId = "researchDataCatalogSourceId",
                SourceDescription = "researchDataCatalogSourceDescription"
            },
            DatasetRelations = null,
            VersionSet = new List<Version>()
            {
                new Version
                {
                    Identifier = "localIdentifier-1",
                    VersionNumber = 1
                },
                new Version
                {
                    Identifier = "localIdentifier-1",
                    VersionNumber = 2
                },
                new Version
                {
                    Identifier = "localIdentifier-2",
                    VersionNumber = 1
                }
            },
            IsLatestVersion = null
        };
    }
}