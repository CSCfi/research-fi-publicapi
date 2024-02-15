using CSC.PublicApi.ElasticService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using CSC.PublicApi.Repositories;
using CSC.PublicApi.Service.Models.FundingCall;
using CSC.PublicApi.Service.Models.Organization;
using Microsoft.Extensions.Caching.Memory;

namespace CSC.PublicApi.Indexer;

public class Indexer
{
    private readonly ILogger<Indexer> _logger;
    private readonly IElasticSearchIndexService _indexService;
    private readonly IConfiguration _configuration;
    private readonly IndexNameSettings _indexNameSettings;
    private readonly IEnumerable<IIndexRepository> _indexRepositories;

    private readonly IMemoryCache _memoryCache;

    public Indexer(
        ILogger<Indexer> logger,
        IElasticSearchIndexService indexService,
        IConfiguration configuration,
        IndexNameSettings indexNameSettings,
        IEnumerable<IIndexRepository> indexRepositories,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _indexService = indexService;
        _configuration = configuration;
        _indexNameSettings = indexNameSettings;
        _indexRepositories = indexRepositories;
        _memoryCache = memoryCache;
    }

    public async Task Start()
    {
        Stopwatch stopWatchMain = new();
        stopWatchMain.Start();

        _logger.LogInformation("Indexing started");
        _logger.LogInformation("Using ElasticSearch at '{ElasticSearchAddress:l}'", _configuration["ELASTICSEARCH:URL"]);

        var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();

        await PopulateOrganizationCache();
        await PopulateFundingCallCache();

        foreach (var (indexName, modelType) in configuredTypesAndIndexNames)
        {
            var repositoryForType = _indexRepositories.SingleOrDefault(repo => repo.ModelType == modelType);

            if (repositoryForType is null)
            {
                _logger.LogError("{EntityType:l}: Unable to find database repository for index {IndexName:l}", modelType.Name, indexName);
                continue;
            }

            _logger.LogInformation("{EntityType:l}: Recreating {IndexName:l} index", modelType.Name, indexName);
            await Task.Delay(1); // Force at least 1 ms separation to log timestamps to preserve log message order in OpenSearch.
            await IndexEntities(indexName, repositoryForType, modelType);
            await Task.Delay(1);
        }

        var totalTime = stopWatchMain.Elapsed;

        _logger.LogInformation("Indexing complete in {Elapsed}", totalTime);
        stopWatchMain.Stop();
    }

    /// <summary>
    /// Gets all organizations from the database to an in-memory cache to simplify SQL queries by automapper.
    /// </summary>
    private async Task PopulateOrganizationCache()
    {
        _logger.LogInformation("Populating Organization cache");
        Stopwatch stopWatchPopulateCache = new();
        stopWatchPopulateCache.Start();
        var organizationRepository = _indexRepositories.SingleOrDefault(repo => repo.ModelType == typeof(Organization));
        if (organizationRepository != null)
        {
            var organizations = await organizationRepository.GetAllAsync().ToListAsync();
            foreach (Organization organization in organizations)
            {
                _memoryCache.Set(MemoryCacheKeys.OrganizationById(organization.Id), organization);
            }
        }
        stopWatchPopulateCache.Stop();
        _logger.LogInformation("Populated Organization cache in {Elapsed}", stopWatchPopulateCache.Elapsed);
    }

    /// <summary>
    /// Gets all call programmes from the database to an in-memory cache to simplify SQL queries by automapper.
    /// </summary>
    private async Task PopulateFundingCallCache()
    {
        _logger.LogInformation("Populating Funding Call cache");
        Stopwatch stopWatchPopulateCache = new();
        stopWatchPopulateCache.Start();
        var fundingCallRepository = _indexRepositories.SingleOrDefault(repo => repo.ModelType == typeof(FundingCall));
        if (fundingCallRepository != null)
        {
            var fundingCalls = await fundingCallRepository.GetAllAsync().ToListAsync();
            foreach (var fundingCall in fundingCalls.Cast<FundingCall>().Where(fundingCall => fundingCall.Id != -1))
            {
                _memoryCache.Set(MemoryCacheKeys.FundingCallBySourceId(fundingCall.SourceId), fundingCall);

                if (fundingCall.SourceProgrammeId == "-1")
                {
                    continue;
                }

                if (_memoryCache.TryGetValue(MemoryCacheKeys.FundingCallByAbbreviationAndEuCallId(fundingCall.Abbreviation, fundingCall.EuCallId), out List<string?> foundFundingCalls))
                {
                    foundFundingCalls.Add(fundingCall.SourceProgrammeId);
                }
                else
                {
                    _memoryCache.Set(MemoryCacheKeys.FundingCallByAbbreviationAndEuCallId(fundingCall.Abbreviation, fundingCall.EuCallId), new List<string?> { fundingCall.SourceProgrammeId });
                }
            }
        }
        stopWatchPopulateCache.Stop();
        _logger.LogInformation("Populated Funding Call cache in {Elapsed}", stopWatchPopulateCache.Elapsed);
    }

    private async Task IndexEntities(string indexName, IIndexRepository repository, Type type)
    {
        Stopwatch stopWatch = new();

        try
        {
            List<object> finalized = new();

            if (indexName.Contains("publication"))
            {
                // Create new index
                var (indexToCreate, indexToDelete) = await _indexService.GetIndexNames(indexName);
                await _indexService.CreateIndex(indexToCreate, type);

                /*
                * Process database result in smaller chunks to keep memory requirement low.
                * Chunking is based on skip/take query.
                * Currently this is done only for publications, because their dataset is much
                * larger than others.
                */

                int skipAmount = 0;
                int takeAmount = 50000;
                int numOfResults = 0;
                int processedCount = 0;

                do
                {
                    _logger.LogInformation("{EntityType:l}: Requesting {TakeAmount} entities from database", type.Name, takeAmount);
                    stopWatch.Start();
                    var indexModels = await repository.GetChunkAsync(skipAmount: skipAmount, takeAmount: takeAmount).ToListAsync();
                    stopWatch.Stop();
                    numOfResults = indexModels.Count;
                    _logger.LogInformation("{EntityType:l}: Received {DatabaseResultCount} entities in {Elapsed}", type.Name, numOfResults, stopWatch.Elapsed);
                    stopWatch.Reset();

                    if (numOfResults > 0)
                    {
                        foreach (object entity in indexModels)
                        {
                            finalized.Add(repository.PerformInMemoryOperation(entity));
                        }
                        await _indexService.IndexChunkAsync(indexToCreate, finalized, type);
                    }
                    skipAmount = skipAmount + takeAmount;
                    processedCount = processedCount + numOfResults;
                    finalized = new();
                    _logger.LogInformation("{EntityType:l}: Total documents indexed = {processedCount}", type.Name, processedCount);
                } while (numOfResults >= takeAmount - 1);

                // Activate new index and delete old
                await _indexService.SwitchIndexes(indexName, indexToCreate, indexToDelete);
                _logger.LogInformation("{EntityType:l}: Recreated {IndexName:l}, {ElasticsearchDocumentCount} documents", type.Name, indexName, processedCount);
            }
            else
            {
                /*
                * Process complete database result at once.
                * Suitable for small result sets.
                */
                _logger.LogInformation("{EntityType:l}: Requested all entities from database", type.Name);
                stopWatch.Start();
                var indexModels = await repository.GetAllAsync().ToListAsync();
                stopWatch.Stop();
                _logger.LogInformation("{EntityType:l}: Received {DatabaseResultCount} entities in {Elapsed}", type.Name, indexModels.Count, stopWatch.Elapsed);

                if (indexModels.Count > 0)
                {
                    finalized = repository.PerformInMemoryOperations(indexModels);
                }

                if (finalized.Count > 0)
                {
                    await _indexService.IndexAsync(indexName, finalized, type);
                    var indexingElapsed = stopWatch.Elapsed;
                    _logger.LogInformation("{EntityType:l}: Recreated {IndexName:l}, {ElasticsearchDocumentCount} documents", type.Name, indexName, finalized.Count);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{EntityType:l}: Exception occurred while indexing {IndexName:l} index after {Elapsed},", type.Name, indexName, stopWatch.Elapsed);
        }
    }
}