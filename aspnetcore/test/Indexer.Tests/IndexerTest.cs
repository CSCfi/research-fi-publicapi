using System.Runtime.CompilerServices;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Indexer;
using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models.FundingCall;
using CSC.PublicApi.Service.Models.FundingDecision;
using CSC.PublicApi.Service.Models.Organization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using Publication = CSC.PublicApi.Service.Models.Publication.Publication;
using FundingCallModel = CSC.PublicApi.Service.Models.FundingCall.FundingCall;
using OrganizationModel = CSC.PublicApi.Service.Models.Organization.Organization;

namespace CSC.PublicApi.Indexer.Tests;

public class IndexerTest
{
    [Theory]
    [InlineData(14999, 1, 2)]
    [InlineData(15000, 1, 2)]
    [InlineData(15001, 2, 2)]
    public async Task Start_IndexesChunkedPublicationBoundariesExactlyOnce(
        int documentCount,
        int expectedBulkCalls,
        int expectedChunkRequests)
    {
        var documents = Enumerable.Range(0, documentCount)
            .Select(index => (object)new Publication { Id = $"publication-{index}" })
            .ToList();
        var repository = new ScriptedRepository(typeof(Publication), documents);
        var indexService = new RecordingIndexService();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = CreateIndexer(
            typeof(Publication),
            "publication",
            repository,
            indexService,
            cache);

        await indexer.Start();

        Assert.Equal(expectedChunkRequests, repository.ChunkRequests.Count);
        Assert.Equal(
            Enumerable.Range(0, expectedChunkRequests).Select(index => index * 15000),
            repository.ChunkRequests.Select(request => request.Skip));
        Assert.All(repository.ChunkRequests, request => Assert.Equal(15000, request.Take));
        Assert.Equal(expectedBulkCalls, indexService.ChunkCalls.Count);
        Assert.Equal(documentCount, indexService.ChunkCalls.Sum(batch => batch.Count));
        Assert.Equal(documentCount, indexService.IndexedIds.Count);
        Assert.Equal(documentCount, indexService.IndexedIds.Distinct().Count());
        Assert.Equal(
            documents.Select(document => ((Publication)document).Id),
            indexService.IndexedIds);
        Assert.Equal(1, indexService.CreateCalls);
        Assert.Equal(1, indexService.SwitchCalls);
        Assert.Empty(indexService.FullCalls);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public async Task Start_IndexesSmallInputOnlyWhenDocumentsExist(int documentCount, int expectedCalls)
    {
        var documents = Enumerable.Range(0, documentCount)
            .Select(index => (object)new FundingDecision { Id = index })
            .ToList();
        var repository = new ScriptedRepository(typeof(FundingDecision), documents);
        var indexService = new RecordingIndexService();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = CreateIndexer(
            typeof(FundingDecision),
            "funding-decision",
            repository,
            indexService,
            cache);

        await indexer.Start();

        Assert.Equal(expectedCalls, indexService.FullCalls.Count);
        Assert.Empty(indexService.ChunkCalls);
        Assert.Empty(repository.ChunkRequests);
        Assert.Equal(documentCount, indexService.IndexedIds.Count);
        Assert.Equal(0, indexService.CreateCalls);
        Assert.Equal(0, indexService.SwitchCalls);
    }

    [Fact]
    public async Task Start_IndexesBulkRepositoryTransformationResults()
    {
        var source = new FundingDecision { Id = 1 };
        var repository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { source })
        {
            BulkTransform = _ => new List<object> { new FundingDecision { Id = 2 } }
        };
        var indexService = new RecordingIndexService();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = CreateIndexer(
            typeof(FundingDecision),
            "funding-decision",
            repository,
            indexService,
            cache);

        await indexer.Start();

        Assert.Equal(1, source.Id);
        Assert.Equal(2, Assert.IsType<FundingDecision>(Assert.Single(indexService.FullCalls.Single())).Id);
        Assert.Equal(new[] { "2" }, indexService.IndexedIds);
    }

    [Fact]
    public async Task Start_SkipsFullIndexWriteWhenBulkTransformationRemovesAllDocuments()
    {
        var source = new FundingDecision { Id = 1 };
        var repository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { source })
        {
            BulkTransform = _ => new List<object>()
        };
        var indexService = new RecordingIndexService();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = CreateIndexer(
            typeof(FundingDecision),
            "funding-decision",
            repository,
            indexService,
            cache);

        await indexer.Start();

        Assert.Equal(1, source.Id);
        Assert.Empty(indexService.FullCalls);
        Assert.Empty(indexService.ChunkCalls);
        Assert.Empty(indexService.IndexedIds);
        Assert.Equal(0, indexService.CreateCalls);
        Assert.Equal(0, indexService.SwitchCalls);
    }

    [Fact]
    public async Task Start_IndexesSingleEntityTransformationResultsForChunkedInput()
    {
        var source = new Publication { Id = "source-publication" };
        var repository = new ScriptedRepository(
            typeof(Publication),
            new List<object> { source })
        {
            SingleTransform = _ => new Publication { Id = "transformed-publication" }
        };
        var indexService = new RecordingIndexService();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = CreateIndexer(
            typeof(Publication),
            "publication",
            repository,
            indexService,
            cache);

        await indexer.Start();

        Assert.Equal("source-publication", source.Id);
        Assert.Equal(
            "transformed-publication",
            Assert.IsType<Publication>(Assert.Single(indexService.ChunkCalls.Single())).Id);
        Assert.Equal(new[] { "transformed-publication" }, indexService.IndexedIds);
        Assert.Equal(1, indexService.SwitchCalls);
    }

    [Fact]
    public async Task Start_DoesNotSwitchIndexWhenChunkRetrievalFails()
    {
        var repository = new ScriptedRepository(typeof(Publication), new List<object>())
        {
            ChunkFailure = new InvalidOperationException("synthetic repository failure")
        };
        var continuationRepository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { new FundingDecision { Id = 1 } });
        var indexService = new RecordingIndexService();
        var logger = new RecordingLogger<Indexer>();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings
            {
                [typeof(Publication).FullName!] = "publication",
                [typeof(FundingDecision).FullName!] = "funding-decision"
            },
            new IIndexRepository[] { repository, continuationRepository },
            cache);

        await indexer.Start();

        Assert.Equal(1, indexService.CreateCalls);
        Assert.Empty(indexService.ChunkCalls);
        Assert.Equal(0, indexService.SwitchCalls);
        Assert.Single(indexService.FullCalls);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("synthetic repository failure"));
    }

    [Fact]
    public async Task Start_DoesNotSwitchPartiallyWrittenIndexWhenLaterChunkRetrievalFails()
    {
        var documents = Enumerable.Range(0, 15000)
            .Select(index => (object)new Publication { Id = $"publication-{index}" })
            .ToList();
        var repository = new ScriptedRepository(typeof(Publication), documents)
        {
            ChunkFailure = new InvalidOperationException("synthetic second-chunk retrieval failure"),
            ChunkFailureSkip = 15000
        };
        var continuationRepository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { new FundingDecision { Id = 1 } });
        var indexService = new RecordingIndexService();
        var logger = new RecordingLogger<Indexer>();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings
            {
                [typeof(Publication).FullName!] = "publication",
                [typeof(FundingDecision).FullName!] = "funding-decision"
            },
            new IIndexRepository[] { repository, continuationRepository },
            cache);

        await indexer.Start();

        Assert.Equal(new[] { 0, 15000 }, repository.ChunkRequests.Select(request => request.Skip));
        Assert.Equal(15000, Assert.Single(indexService.ChunkCalls).Count);
        Assert.Equal(15001, indexService.IndexedIds.Count);
        Assert.Equal(15001, indexService.IndexedIds.Distinct().Count());
        Assert.Equal(
            documents.Select(document => ((Publication)document).Id),
            indexService.IndexedIds.Take(15000));
        Assert.Equal("1", indexService.IndexedIds.Last());
        Assert.Single(indexService.FullCalls);
        Assert.Equal(0, indexService.SwitchCalls);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error &&
            entry.Message.Contains("synthetic second-chunk retrieval failure"));
    }

    [Fact]
    public async Task Start_DoesNotSwitchIndexAndContinuesWhenChunkIndexingFails()
    {
        var repository = new ScriptedRepository(
            typeof(Publication),
            new List<object> { new Publication { Id = "publication-1" } });
        var continuationRepository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { new FundingDecision { Id = 1 } });
        var indexService = new RecordingIndexService
        {
            ChunkFailure = new InvalidOperationException("synthetic indexing failure")
        };
        var logger = new RecordingLogger<Indexer>();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings
            {
                [typeof(Publication).FullName!] = "publication",
                [typeof(FundingDecision).FullName!] = "funding-decision"
            },
            new IIndexRepository[] { repository, continuationRepository },
            cache);

        await indexer.Start();

        Assert.Single(indexService.ChunkCalls);
        Assert.Single(indexService.FullCalls);
        Assert.Equal(new[] { "publication-1", "1" }, indexService.IndexedIds);
        Assert.Equal(0, indexService.SwitchCalls);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("synthetic indexing failure"));
    }

    [Fact]
    public async Task Start_DoesNotRequestChunksOrSwitchWhenIndexCreationFails()
    {
        var repository = new ScriptedRepository(
            typeof(Publication),
            new List<object> { new Publication { Id = "publication-1" } });
        var continuationRepository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { new FundingDecision { Id = 1 } });
        var indexService = new RecordingIndexService
        {
            CreateFailure = new InvalidOperationException("synthetic index creation failure")
        };
        var logger = new RecordingLogger<Indexer>();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings
            {
                [typeof(Publication).FullName!] = "publication",
                [typeof(FundingDecision).FullName!] = "funding-decision"
            },
            new IIndexRepository[] { repository, continuationRepository },
            cache);

        await indexer.Start();

        Assert.Equal(1, indexService.CreateCalls);
        Assert.Empty(repository.ChunkRequests);
        Assert.Empty(indexService.ChunkCalls);
        Assert.Equal(0, indexService.SwitchCalls);
        Assert.Single(indexService.FullCalls);
        Assert.Equal("1", Assert.Single(indexService.IndexedIds));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("synthetic index creation failure"));
    }

    [Fact]
    public async Task Start_DoesNotCreateIndexOrRequestChunksWhenIndexNameResolutionFails()
    {
        var repository = new ScriptedRepository(
            typeof(Publication),
            new List<object> { new Publication { Id = "publication-1" } });
        var continuationRepository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { new FundingDecision { Id = 1 } });
        var indexService = new RecordingIndexService
        {
            GetIndexNamesFailure = new InvalidOperationException("synthetic index-name failure")
        };
        var logger = new RecordingLogger<Indexer>();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings
            {
                [typeof(Publication).FullName!] = "publication",
                [typeof(FundingDecision).FullName!] = "funding-decision"
            },
            new IIndexRepository[] { repository, continuationRepository },
            cache);

        await indexer.Start();

        Assert.Equal(1, indexService.GetIndexNamesCalls);
        Assert.Equal(0, indexService.CreateCalls);
        Assert.Empty(repository.ChunkRequests);
        Assert.Empty(indexService.ChunkCalls);
        Assert.Equal(0, indexService.SwitchCalls);
        Assert.Single(indexService.FullCalls);
        Assert.Equal("1", Assert.Single(indexService.IndexedIds));
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("synthetic index-name failure"));
    }

    [Fact]
    public async Task Start_LogsAndContinuesWhenFullIndexWriteFails()
    {
        var failedRepository = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { new FundingDecision { Id = 99 } });
        var continuationRepository = new ScriptedRepository(
            typeof(Publication),
            new List<object> { new Publication { Id = "publication-after-failure" } });
        var indexService = new RecordingIndexService
        {
            FullFailure = new InvalidOperationException("synthetic full-index failure")
        };
        var logger = new RecordingLogger<Indexer>();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings
            {
                [typeof(FundingDecision).FullName!] = "funding-decision",
                [typeof(Publication).FullName!] = "publication"
            },
            new IIndexRepository[] { failedRepository, continuationRepository },
            cache);

        await indexer.Start();

        Assert.Single(indexService.FullCalls);
        Assert.Equal(99, Assert.IsType<FundingDecision>(Assert.Single(indexService.FullCalls[0])).Id);
        Assert.Equal("publication-after-failure", Assert.Single(indexService.ChunkCalls)
            .Select(document => Assert.IsType<Publication>(document).Id).Single());
        Assert.Equal(new[] { "99", "publication-after-failure" }, indexService.IndexedIds);
        Assert.Equal(1, indexService.SwitchCalls);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("synthetic full-index failure"));
    }

    [Fact]
    public async Task Start_LogsAndContinuesWhenFullRepositoryRetrievalFails()
    {
        var failedRepository = new ScriptedRepository(
            typeof(FundingDecision),
            Array.Empty<object>())
        {
            AllFailure = new InvalidOperationException("synthetic full-repository failure")
        };
        var continuationRepository = new ScriptedRepository(
            typeof(Publication),
            new List<object> { new Publication { Id = "publication-after-retrieval-failure" } });
        var indexService = new RecordingIndexService();
        var logger = new RecordingLogger<Indexer>();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings
            {
                [typeof(FundingDecision).FullName!] = "funding-decision",
                [typeof(Publication).FullName!] = "publication"
            },
            new IIndexRepository[] { failedRepository, continuationRepository },
            cache);

        await indexer.Start();

        Assert.Empty(indexService.FullCalls);
        Assert.Equal("publication-after-retrieval-failure",
            Assert.IsType<Publication>(Assert.Single(indexService.ChunkCalls.Single())).Id);
        Assert.Equal(new[] { "publication-after-retrieval-failure" }, indexService.IndexedIds);
        Assert.Equal(1, indexService.SwitchCalls);
        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error && entry.Message.Contains("synthetic full-repository failure"));
    }

    [Fact]
    public async Task Start_PopulatesOrganizationAndFundingCallCachesBeforeIndexing()
    {
        var organizations = new ScriptedRepository(typeof(OrganizationModel), new object[]
        {
            new OrganizationModel { Id = 42, OrganizationId = "org-42" }
        });
        var fundingCalls = new ScriptedRepository(typeof(FundingCallModel), new object[]
        {
            new FundingCallModel
            {
                Id = 1,
                SourceId = "source-1",
                Abbreviation = "EU",
                EuCallId = "call-1",
                SourceProgrammeId = "programme-1"
            },
            new FundingCallModel
            {
                Id = 2,
                SourceId = "source-2",
                Abbreviation = "EU",
                EuCallId = "call-1",
                SourceProgrammeId = "programme-2"
            },
            new FundingCallModel
            {
                Id = 3,
                SourceId = "source-without-programme",
                Abbreviation = "EU",
                EuCallId = "call-2",
                SourceProgrammeId = "-1"
            },
            new FundingCallModel
            {
                Id = -1,
                SourceId = "sentinel",
                Abbreviation = "EU",
                EuCallId = "call-1",
                SourceProgrammeId = "sentinel-programme"
            }
        });
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var observingRepository = new CacheObservingRepository(cache);
        var indexService = new RecordingIndexService();
        var indexer = new Indexer(
            NullLogger<Indexer>.Instance,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings { [typeof(FundingDecision).FullName!] = "funding-decision" },
            new IIndexRepository[] { organizations, fundingCalls, observingRepository },
            cache);

        await indexer.Start();

        Assert.True(observingRepository.CacheWasPopulatedBeforeProcessing);
        Assert.True(cache.TryGetValue(MemoryCacheKeys.FundingCallBySourceId("source-1"), out FundingCallModel? first));
        Assert.Equal("source-1", first?.SourceId);
        Assert.True(cache.TryGetValue(MemoryCacheKeys.FundingCallBySourceId("source-without-programme"), out _));
        Assert.False(cache.TryGetValue(MemoryCacheKeys.FundingCallBySourceId("sentinel"), out _));
        Assert.True(cache.TryGetValue(
            MemoryCacheKeys.FundingCallByAbbreviationAndEuCallId("EU", "call-1"),
            out List<string?>? programmes));
        Assert.NotNull(programmes);
        Assert.Equal(new[] { "programme-1", "programme-2" }, programmes);
        Assert.False(cache.TryGetValue(
            MemoryCacheKeys.FundingCallByAbbreviationAndEuCallId("EU", "call-2"),
            out _));
        Assert.Single(indexService.FullCalls);
    }

    [Fact]
    public async Task Start_LogsAndContinuesWhenConfiguredRepositoryIsMissing()
    {
        var logger = new RecordingLogger<Indexer>();
        var indexService = new RecordingIndexService();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            logger,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings { [typeof(FundingDecision).FullName!] = "funding-decision" },
            Array.Empty<IIndexRepository>(),
            cache);

        await indexer.Start();

        Assert.Contains(logger.Entries, entry =>
            entry.Level == LogLevel.Error &&
            entry.Message.Contains("Unable to find database repository") &&
            entry.Message.Contains("funding-decision"));
        Assert.Empty(indexService.FullCalls);
        Assert.Empty(indexService.ChunkCalls);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Start_PropagatesCacheRepositoryFailuresBeforeIndexing(bool failOrganizationCache)
    {
        var failure = new InvalidOperationException("synthetic cache repository failure");
        var organizations = new ScriptedRepository(
            typeof(OrganizationModel),
            Array.Empty<object>())
        {
            AllFailure = failOrganizationCache ? failure : null
        };
        var fundingCalls = new ScriptedRepository(
            typeof(FundingCallModel),
            Array.Empty<object>())
        {
            AllFailure = failOrganizationCache ? null : failure
        };
        var target = new ScriptedRepository(
            typeof(FundingDecision),
            new List<object> { new FundingDecision { Id = 1 } });
        var indexService = new RecordingIndexService();
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var indexer = new Indexer(
            NullLogger<Indexer>.Instance,
            indexService,
            new ConfigurationBuilder().Build(),
            new IndexNameSettings { [typeof(FundingDecision).FullName!] = "funding-decision" },
            new IIndexRepository[] { organizations, fundingCalls, target },
            cache);

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() => indexer.Start());

        Assert.Same(failure, thrown);
        Assert.Empty(indexService.FullCalls);
        Assert.Empty(indexService.ChunkCalls);
    }

    private static Indexer CreateIndexer(
        Type modelType,
        string indexName,
        ScriptedRepository repository,
        RecordingIndexService indexService,
        IMemoryCache cache)
    {
        var indexSettings = new IndexNameSettings { [modelType.FullName!] = indexName };
        return new Indexer(
            NullLogger<Indexer>.Instance,
            indexService,
            new ConfigurationBuilder().Build(),
            indexSettings,
            new[] { repository },
            cache);
    }

    private sealed class ScriptedRepository(Type modelType, IReadOnlyList<object> documents) : IIndexRepository
    {
        public Type ModelType { get; } = modelType;
        public List<(int Skip, int Take)> ChunkRequests { get; } = new();
        public Exception? AllFailure { get; init; }
        public Exception? ChunkFailure { get; init; }
        public int? ChunkFailureSkip { get; init; }
        public Func<List<object>, List<object>>? BulkTransform { get; init; }
        public Func<object, object>? SingleTransform { get; init; }

        public IAsyncEnumerable<object> GetAllAsync()
        {
            if (AllFailure is not null)
                throw AllFailure;
            return Enumerate(documents);
        }

        public IAsyncEnumerable<object> GetChunkAsync(int skipAmount, int takeAmount)
        {
            ChunkRequests.Add((skipAmount, takeAmount));
            if (ChunkFailure is not null && (ChunkFailureSkip is null || ChunkFailureSkip == skipAmount))
                throw ChunkFailure;
            return Enumerate(documents.Skip(skipAmount).Take(takeAmount));
        }

        public List<object> PerformInMemoryOperations(List<object> objects) =>
            BulkTransform?.Invoke(objects) ?? objects;

        public object PerformInMemoryOperation(object entity) =>
            SingleTransform?.Invoke(entity) ?? entity;

        public static async IAsyncEnumerable<object> Enumerate(
            IEnumerable<object> entities,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return entity;
                await Task.Yield();
            }
        }
    }

    private sealed class CacheObservingRepository(IMemoryCache cache) : IIndexRepository
    {
        public Type ModelType => typeof(FundingDecision);
        public bool CacheWasPopulatedBeforeProcessing { get; private set; }

        public IAsyncEnumerable<object> GetAllAsync() =>
            ScriptedRepository.Enumerate(new object[] { new FundingDecision { Id = 5 } });

        public IAsyncEnumerable<object> GetChunkAsync(int skipAmount, int takeAmount) =>
            ScriptedRepository.Enumerate(Array.Empty<object>());

        public List<object> PerformInMemoryOperations(List<object> objects)
        {
            CacheWasPopulatedBeforeProcessing =
                cache.TryGetValue(MemoryCacheKeys.OrganizationById(42), out OrganizationModel? organization) &&
                organization?.OrganizationId == "org-42" &&
                cache.TryGetValue(
                    MemoryCacheKeys.FundingCallByAbbreviationAndEuCallId("EU", "call-1"),
                    out List<string?>? programmes) &&
                programmes?.Count == 2;
            return objects;
        }

        public object PerformInMemoryOperation(object entity) => entity;
    }

    private sealed class RecordingIndexService : IElasticSearchIndexService
    {
        public int CreateCalls { get; private set; }
        public int GetIndexNamesCalls { get; private set; }
        public int SwitchCalls { get; private set; }
        public Exception? GetIndexNamesFailure { get; init; }
        public Exception? CreateFailure { get; init; }
        public Exception? FullFailure { get; init; }
        public Exception? ChunkFailure { get; init; }
        public List<List<object>> FullCalls { get; } = new();
        public List<List<object>> ChunkCalls { get; } = new();
        public List<string?> IndexedIds { get; } = new();

        public Task IndexAsync(string indexName, List<object> entities, Type modelType)
        {
            FullCalls.Add(entities);
            RecordIds(entities);
            if (FullFailure is not null)
                throw FullFailure;
            return Task.CompletedTask;
        }

        public Task IndexChunkAsync(string indexName, List<object> entities, Type modelType)
        {
            ChunkCalls.Add(entities);
            RecordIds(entities);
            if (ChunkFailure is not null)
                throw ChunkFailure;
            return Task.CompletedTask;
        }

        public Task<(string indexToCreate, string indexToDelete)> GetIndexNames(string indexName)
        {
            GetIndexNamesCalls++;
            if (GetIndexNamesFailure is not null)
                throw GetIndexNamesFailure;
            return Task.FromResult(($"{indexName}-new", $"{indexName}-old"));
        }

        public Task CreateIndex(string indexName, Type type)
        {
            CreateCalls++;
            if (CreateFailure is not null)
                throw CreateFailure;
            return Task.CompletedTask;
        }

        public Task SwitchIndexes(string indexName, string indexToCreate, string indexToDelete, string modelTypeName)
        {
            SwitchCalls++;
            return Task.CompletedTask;
        }

        private void RecordIds(IEnumerable<object> entities)
        {
            IndexedIds.AddRange(entities.Select(entity => entity switch
            {
                Publication publication => publication.Id,
                FundingDecision fundingDecision => fundingDecision.Id.ToString(),
                _ => throw new ArgumentOutOfRangeException(nameof(entity))
            }));
        }
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<(LogLevel Level, string Message)> Entries { get; } = new();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add((logLevel, formatter(state, exception)));
        }
    }
}
