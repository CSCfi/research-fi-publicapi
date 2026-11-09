using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.FundingCall;
using CSC.PublicApi.Service.Models.Infrastructure;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Xunit;
using FundingCallModel = CSC.PublicApi.Service.Models.FundingCall.FundingCall;
using FundingDecisionModel = CSC.PublicApi.Service.Models.FundingDecision.FundingDecision;
using InfrastructureModel = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;
using OrganizationModel = CSC.PublicApi.Service.Models.Organization.Organization;
using PublicationModel = CSC.PublicApi.Service.Models.Publication.Publication;
using ResearchDatasetModel = CSC.PublicApi.Service.Models.ResearchDataset.ResearchDataset;
using InfrastructureServiceModel = CSC.PublicApi.Service.Models.Infrastructure.Service;

namespace CSC.PublicApi.Interface.Tests;

[CollectionDefinition("Elasticsearch API integration")]
public sealed class ElasticsearchApiCollection : ICollectionFixture<ElasticsearchApiFixture>
{
    public const string Name = "Elasticsearch API integration";
}

public sealed class ElasticsearchApiFixture : IAsyncLifetime
{
    private readonly Dictionary<Type, string> _indices = new();
    private readonly List<string> _createdIndices = new();
    private readonly Dictionary<string, int> _expectedDocumentCounts = new();
    private readonly IContainer _elasticsearchContainer = new ContainerBuilder(
            "docker.elastic.co/elasticsearch/elasticsearch:8.19.22")
        .WithPortBinding(9200, true)
        .WithEnvironment("discovery.type", "single-node")
        .WithEnvironment("xpack.security.enabled", "false")
        .WithEnvironment("ES_JAVA_OPTS", "-Xms512m -Xmx512m")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(
            request => request.ForPort(9200).ForPath("/_cluster/health")))
        .Build();

    public TestWebApplicationFactory<Program> Factory { get; private set; } = null!;

    public IElasticClient ElasticClient { get; private set; } = null!;

    public string FundingCallIndex => _indices[typeof(FundingCallModel)];

    public string FundingDecisionIndex => _indices[typeof(FundingDecisionModel)];

    public string InfrastructureIndex => _indices[typeof(InfrastructureModel)];

    public string OrganizationIndex => _indices[typeof(OrganizationModel)];

    public string ResearchDatasetIndex => _indices[typeof(ResearchDatasetModel)];

    public string PublicationIndex => _indices[typeof(PublicationModel)];

    public string InfrastructureServiceIndex => _indices[typeof(InfrastructureServiceModel)];

    public async Task InitializeAsync()
    {
        await _elasticsearchContainer.StartAsync();
        var elasticsearchUrl = new UriBuilder(
            "http",
            _elasticsearchContainer.Hostname,
            _elasticsearchContainer.GetMappedPublicPort(9200)).Uri.ToString();

        var runId = Guid.NewGuid().ToString("N");
        AddIndex<FundingCallModel>($"researchfi-test-{runId}-funding-call");
        AddIndex<FundingDecisionModel>($"researchfi-test-{runId}-funding-decision");
        AddIndex<InfrastructureModel>($"researchfi-test-{runId}-infrastructure");
        AddIndex<OrganizationModel>($"researchfi-test-{runId}-organization");
        AddIndex<ResearchDatasetModel>($"researchfi-test-{runId}-research-dataset");
        AddIndex<PublicationModel>($"researchfi-test-{runId}-publication");
        AddIndex<InfrastructureServiceModel>($"researchfi-test-{runId}-infrastructure-service");

        Factory = new TestWebApplicationFactory<Program>
        {
            UseRealElasticsearch = true,
            ElasticsearchUrl = elasticsearchUrl,
            IndexNames = new Dictionary<string, string>
            {
                [typeof(FundingCallModel).FullName!] = FundingCallIndex,
                [typeof(FundingDecisionModel).FullName!] = FundingDecisionIndex,
                [typeof(InfrastructureModel).FullName!] = InfrastructureIndex,
                [typeof(OrganizationModel).FullName!] = OrganizationIndex,
                [typeof(ResearchDatasetModel).FullName!] = ResearchDatasetIndex,
                [typeof(PublicationModel).FullName!] = PublicationIndex,
                [typeof(InfrastructureServiceModel).FullName!] = InfrastructureServiceIndex
            },
            AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
            Roles = ApiPolicies.PolicyRoleMap.Values.Distinct().ToArray()
        };

        using var client = Factory.CreateClient();
        ElasticClient = Factory.Services.GetRequiredService<IElasticClient>();
        await VerifyClusterVersionAsync();
        VerifyHostConfiguration();

        foreach (var (modelType, indexName) in _indices)
        {
            var response = await ElasticClient.Indices.CreateAsync(
                indexName,
                descriptor => descriptor.Map(mapping => mapping.AutoMap(modelType, maxRecursion: 1)));
            EnsureValid(response, $"Creating test index '{indexName}'");
            _createdIndices.Add(indexName);
        }

        await IndexAsync(FundingCallIndex, "funding-call-1001", new FundingCallModel
        {
            ExportSortId = 1,
            Id = 1001,
            NameFi = "Phase Three Synthetic Funding Call",
            Foundations = new List<Foundation>
            {
                new()
                {
                    NameFi = "Phase Three Synthetic Foundation",
                    BusinessId = "1234567-8"
                }
            },
            Categories = new List<CSC.PublicApi.Service.Models.ReferenceData>
            {
                new() { Code = "PHASE-3" }
            },
            CallProgrammeOpenDate = new DateTime(2025, 1, 1),
            CallProgrammeDueDate = new DateTime(2025, 12, 31)
        });
        await IndexAsync(FundingDecisionIndex, "funding-decision-2001", new FundingDecisionModel
        {
            ExportSortId = 2,
            Id = 2001,
            NameEn = "Phase Three Synthetic Funding Decision",
            DescriptionEn = "Synthetic integration fixture",
            Funder = new CSC.PublicApi.Service.Models.FundingDecision.Organization
            {
                NameEn = "Phase Three Synthetic Funder",
                Pids = new List<CSC.PublicApi.Service.Models.PersistentIdentifier>
                {
                    new() { Type = "businessid", Content = "7654321-0" }
                }
            },
            FunderProjectNumber = "PHASE-3-PROJECT",
            TypeOfFunding = new CSC.PublicApi.Service.Models.ReferenceData { Code = "PHASE-3-TYPE" }
        });
        await IndexAsync(InfrastructureIndex, "infrastructure-3001", new InfrastructureModel
        {
            ExportSortId = 3,
            InfraIdentifier = new Identifier { LocalIdentifier = "phase-3-infrastructure" },
            InfraName = new List<DescriptiveText>
            {
                new() { TextContent = "Phase Three Synthetic Infrastructure", TextLanguage = "en" }
            },
            InfraAcronym = "P3INFRA"
        });
        await IndexAsync(OrganizationIndex, "organization-4001", new OrganizationModel
        {
            Id = 4001,
            OrganizationId = "phase-3-organization",
            NameEn = "Phase Three Synthetic Organization"
        });
        await IndexAsync(ResearchDatasetIndex, "dataset-phase-3", new ResearchDatasetModel
        {
            ExportSortId = 5,
            Id = "dataset-phase-3",
            NameEn = "Phase Three Synthetic Research Dataset",
            IsLatestVersion = true
        });
        await SeedPublicationsAsync();
        await SeedInfrastructureServicesAsync();

        foreach (var indexName in _createdIndices)
        {
            var refreshResponse = await ElasticClient.Indices.RefreshAsync(indexName);
            EnsureValid(refreshResponse, $"Refreshing test index '{indexName}'");
        }

        await VerifyIndexedDocumentsAsync();
    }

    public async Task DisposeAsync()
    {
        try
        {
            if (ElasticClient is not null)
            {
                foreach (var indexName in _createdIndices)
                {
                    var response = await ElasticClient.Indices.DeleteAsync(indexName);
                    EnsureValid(response, $"Deleting test index '{indexName}'");
                }
            }
        }
        finally
        {
            Factory?.Dispose();
            await _elasticsearchContainer.DisposeAsync();
        }
    }

    private async Task VerifyClusterVersionAsync()
    {
        var response = await ElasticClient.RootNodeInfoAsync();
        EnsureValid(response, "Connecting to the Testcontainers Elasticsearch instance");

        var version = response.Version?.Number;
        if (version is null || !version.StartsWith("8.19.", StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Elasticsearch integration tests require version 8.19.x; connected to '{version ?? "unknown"}'.");
        }
    }

    private void VerifyHostConfiguration()
    {
        var indexNames = Factory.Services.GetRequiredService<IndexNameSettings>();
        foreach (var (modelType, indexName) in _indices)
        {
            if (indexNames.GetIndexNameForType(modelType) != indexName)
            {
                throw new InvalidOperationException(
                    $"The API host resolved an unexpected index for '{modelType.FullName}'.");
            }
        }

        var searchService = Factory.Services.GetRequiredService<
            ISearchService<FundingCallSearchParameters, FundingCallModel>>();
        if (searchService is not ElasticSearchService<FundingCallSearchParameters, FundingCallModel>)
        {
            throw new InvalidOperationException(
                $"The integration host resolved '{searchService.GetType().FullName}' instead of the real Elasticsearch search service.");
        }
    }

    private async Task VerifyIndexedDocumentsAsync()
    {
        foreach (var indexName in _createdIndices)
        {
            var response = await ElasticClient.CountAsync<object>(descriptor => descriptor.Index(indexName));
            EnsureValid(response, $"Counting documents in test index '{indexName}'");
            var expectedCount = _expectedDocumentCounts[indexName];
            if (response.Count != expectedCount)
            {
                throw new InvalidOperationException(
                    $"Expected {expectedCount} synthetic documents in test index '{indexName}', but found {response.Count}.");
            }
        }
    }

    private void AddIndex<T>(string indexName)
    {
        _indices.Add(typeof(T), indexName);
        _expectedDocumentCounts.Add(indexName, 0);
    }

    private async Task IndexAsync<T>(string indexName, string documentId, T document) where T : class
    {
        var response = await ElasticClient.IndexAsync(
            document,
            descriptor => descriptor.Index(indexName).Id(documentId));
        EnsureValid(response, $"Indexing document '{documentId}' in '{indexName}'");
        _expectedDocumentCounts[indexName]++;
    }

    private async Task SeedPublicationsAsync()
    {
        var publication = CreatePublication("publication-main", 10, 2025);
        publication.Doi = "10.9999/synthetic-publication";
        await IndexAsync(PublicationIndex, "publication-document-7001", publication);

        var organizationPart = CreatePublication("publication-part", 20, 2025);
        organizationPart.IsOrgPublication = true;
        organizationPart.CoPublicationID = "publication-co";
        await IndexAsync(PublicationIndex, organizationPart.Id!, organizationPart);

        var splitAuthors = CreatePublication("publication-split-authors", 30, 2025);
        splitAuthors.Authors = new()
        {
            new() { FirstNames = "Synthetic", LastName = "Other" },
            new() { FirstNames = "Other", LastName = "Researcher" }
        };
        await IndexAsync(PublicationIndex, splitAuthors.Id!, splitAuthors);

        var copublication = CreatePublication("publication-co", 40, 2024);
        copublication.IsCoPublication = true;
        copublication.OrgPublicationIDs = new() { "publication-part" };
        copublication.OrganizationPartsOfCoPublication = new()
        {
            new()
            {
                Id = "publication-part",
                OriginalPublicationId = "original-publication-part",
                Organization = new() { Id = "synthetic-organization" }
            }
        };
        await IndexAsync(PublicationIndex, copublication.Id!, copublication);

        var olderPublication = CreatePublication("publication-older", 50, 2023);
        await IndexAsync(PublicationIndex, olderPublication.Id!, olderPublication);
    }

    private static PublicationModel CreatePublication(string id, long sortId, int year)
    {
        return new PublicationModel
        {
            Id = id,
            ExportSortId = sortId,
            OriginalPublicationId = $"original-{id}",
            Name = "Phase Three Synthetic Publication",
            PublicationYear = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Type = new() { Code = "A1" },
            Authors = new()
            {
                new() { FirstNames = "Synthetic", LastName = "Researcher", Orcid = "synthetic-orcid" }
            }
        };
    }

    private async Task SeedInfrastructureServicesAsync()
    {
        foreach (var id in new[] { 6001, 6002, 6003 })
        {
            await IndexAsync(InfrastructureServiceIndex, $"service-{id}", new InfrastructureServiceModel
            {
                ExportSortId = id,
                ServiceIdentifier = new Identifier
                {
                    LocalIdentifier = $"service-{id}",
                    KeyIdentifierURN = $"urn:synthetic:service:{id}",
                    OtherPid = new() { new() { PidType = "synthetic", PidContent = $"service-pid-{id}" } }
                },
                ServiceName = new() { new() { TextContent = "Phase Three Synthetic Imaging", TextLanguage = "en" } },
                ServiceDescription = new()
                {
                    new()
                    {
                        TextContent = id == 6002 ? "Chemical analysis" : "Precision imaging",
                        TextLanguage = "en"
                    }
                },
                ServiceStartsOn = new InfraDate(2020, 1, 1),
                ServiceEndsOn = new InfraDate(id == 6003 ? 2031 : 2030, 12, 31),
                IsPartOfInfrastructure = new ServiceInfrastructure
                {
                    InfraIdentifier = new Identifier
                    {
                        KeyIdentifierURN = id == 6003 ? "urn:synthetic:infra:other" : "urn:synthetic:infra:main"
                    },
                    InfraResponsibleOrganization = new ResearchOrganization
                    {
                        OrganizationIdentifier = new()
                        {
                            new() { PidType = "businessid", PidContent = "1234567-8" }
                        }
                    }
                }
            });
        }
    }

    private static void EnsureValid(IResponse response, string operation)
    {
        if (!response.IsValid)
        {
            throw new InvalidOperationException($"{operation} failed: {response.DebugInformation}");
        }
    }
}
