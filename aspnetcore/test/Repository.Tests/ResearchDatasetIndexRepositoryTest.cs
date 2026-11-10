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
    
    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    public void PerformInMemoryOperations_ResearchDataset_ComparesVersionInfoWithLatestVersion(
        int versionInfo, bool expectedIsLatestVersion)
    {
        var researchDataset = GetModel();
        researchDataset.VersionInfo = versionInfo;
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

        _researchDatasetIndexRepository.PerformInMemoryOperations(new List<object> { researchDataset });

        researchDataset.VersionSet.Should().NotBeNull();
        researchDataset.IsLatestVersion.Should().Be(expectedIsLatestVersion);

        researchDataset.ResearchfiUrl.Should().NotBeNull();
        researchDataset.ResearchfiUrl.Fi.Should().Be("https://tiedejatutkimus.fi/fi/results/dataset/localIdentifier-1");
        researchDataset.ResearchfiUrl.Sv.Should().Be("https://forskning.fi/sv/results/dataset/localIdentifier-1");
        researchDataset.ResearchfiUrl.En.Should().Be("https://research.fi/en/results/dataset/localIdentifier-1");
    }

    [Fact]
    public void PerformInMemoryOperations_ResearchDatasetWithEmptyVersionSet_IsLatestAndClearsVersionSet()
    {
        var researchDataset = GetModel();
        researchDataset.VersionSet = new List<Version>();

        _researchDatasetIndexRepository.PerformInMemoryOperations(new List<object> { researchDataset });

        researchDataset.IsLatestVersion.Should().BeTrue();
        researchDataset.VersionSet.Should().BeNull();
    }

    [Theory]
    [InlineData("direct")]
    [InlineData("identifierless-organization")]
    [InlineData("identifierless-data")]
    public void PerformInMemoryOperation_ResolvesContributorOrganizationByRelationshipPriority(string relationship)
    {
        var directOrganization = GetOrganization("direct");
        var identifierlessOrganization = GetOrganization("identifierless");
        var identifierlessDataOrganization = GetOrganization("data");
        var helper = new ContributorHelper
        {
            FactContribution_DimOrganizationId = -1,
            FactContribution_DimIdentifierlessDataId = -1,
            FactContribution_DimIdentifierlessData_DimOrganizationId = -1,
            Organization_From_FactContribution_DimOrganization = directOrganization,
            Organization_From_FactContribution_DimIdentifierlessData_DimOrganization = identifierlessOrganization,
            Organization_From_FactContribution_DimIdentifierlessData = identifierlessDataOrganization,
            Person = new Person()
        };

        var expectedOrganization = relationship switch
        {
            "direct" => directOrganization,
            "identifierless-organization" => identifierlessOrganization,
            "identifierless-data" => identifierlessDataOrganization,
            _ => throw new ArgumentOutOfRangeException(nameof(relationship))
        };

        if (relationship == "direct")
        {
            helper.FactContribution_DimOrganizationId = 1;
        }
        else
        {
            helper.FactContribution_DimIdentifierlessDataId = 2;
            if (relationship == "identifierless-organization")
                helper.FactContribution_DimIdentifierlessData_DimOrganizationId = 3;
        }

        var dataset = new ResearchDataset
        {
            Id = "with-contributor",
            ContributorsHelper = new List<ContributorHelper> { helper },
            VersionSet = null
        };

        var result = _researchDatasetIndexRepository.PerformInMemoryOperation(dataset);

        result.Should().BeSameAs(dataset);
        dataset.Contributors.Should().ContainSingle()
            .Which.Organization.Should().BeSameAs(expectedOrganization);
        dataset.ContributorsHelper.Should().BeNull();
    }

    [Fact]
    public void PerformInMemoryOperations_RemovesContributorsWithoutPersonOrRealOrganization()
    {
        var helperWithoutRealOrganization = new ContributorHelper
        {
            FactContribution_DimOrganizationId = 1,
            FactContribution_DimIdentifierlessDataId = -1,
            FactContribution_DimIdentifierlessData_DimOrganizationId = -1,
            Organization_From_FactContribution_DimOrganization = GetOrganization("-1")
        };
        var helperWithoutRelationships = new ContributorHelper
        {
            FactContribution_DimOrganizationId = -1,
            FactContribution_DimIdentifierlessDataId = -1,
            FactContribution_DimIdentifierlessData_DimOrganizationId = -1
        };
        var personOnlyHelper = new ContributorHelper
        {
            FactContribution_DimOrganizationId = 1,
            FactContribution_DimIdentifierlessDataId = -1,
            FactContribution_DimIdentifierlessData_DimOrganizationId = -1,
            Person = new Person()
        };
        var dataset = new ResearchDataset
        {
            Id = "contributors",
            ContributorsHelper = new List<ContributorHelper>
            {
                helperWithoutRealOrganization,
                helperWithoutRelationships,
                personOnlyHelper
            }
        };

        _researchDatasetIndexRepository.PerformInMemoryOperations(new List<object> { dataset });

        dataset.Contributors.Should().ContainSingle()
            .Which.Person.Should().BeSameAs(personOnlyHelper.Person);
        dataset.Contributors!.Single().Organization.Should().BeNull();
        dataset.ContributorsHelper.Should().BeNull();
    }

    private static Organization GetOrganization(string id) => new() { Id = id };
    
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