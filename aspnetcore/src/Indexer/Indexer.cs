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
        Stopwatch stopWatch = new();
        stopWatch.Start();

        _logger.LogInformation("Starting indexing...");
        _logger.LogInformation("Using ElasticSearch at '{ElasticSearchAddress}'", _configuration["ELASTICSEARCH:URL"]);

        var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();

        await PopulateOrganizationCache();
        await PopulateFundingCallCache();

        foreach (var (indexName, modelType) in configuredTypesAndIndexNames)
        {
            var repositoryForType = _indexRepositories.SingleOrDefault(repo => repo.ModelType == modelType);

            if (repositoryForType is null)
            {
                _logger.LogError("{EntityType}: Unable to find database repository for index {IndexName}", modelType.Name, indexName);      
                continue;
            }

            await IndexEntities(indexName, repositoryForType, modelType);
        }

        var totalTime = stopWatch.Elapsed;

        _logger.LogInformation("Indexing completed in {Elapsed}, finishing process", totalTime);
        stopWatch.Stop();
    }

    /// <summary>
    /// Gets all organizations from the database to an in-memory cache to simplify SQL queries by automapper.
    /// </summary>
    private async Task PopulateOrganizationCache()
    {
        _logger.LogInformation("Populating Organization cache");

        var organizationRepository = _indexRepositories.SingleOrDefault(repo => repo.ModelType == typeof(Organization));
        if (organizationRepository != null)
        {
            var organizations = await organizationRepository.GetAllAsync().ToListAsync();
            foreach (Organization organization in organizations)
            {
                _memoryCache.Set(MemoryCacheKeys.OrganizationById(organization.Id), organization);
            }
        }

        _logger.LogInformation("Populated Organization cache");
    }
    
    /// <summary>
    /// Gets all call programmes from the database to an in-memory cache to simplify SQL queries by automapper.
    /// </summary>
    private async Task PopulateFundingCallCache()
    {
        _logger.LogInformation("Populating Funding Call cache");

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

        _logger.LogInformation("Populated Funding Call cache");
    }

    private async Task IndexEntities(string indexName,
        IIndexRepository repository,
        Type type)
    {
        _logger.LogInformation("{EntityType}: Recreating '{IndexName}' index...", type.Name, indexName);

        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            List<object> finalized = new();

            if (indexName.Contains("publication")) {
                /*
                * Process database result in smaller chunks to keep memory requirement low.
                * Chunking is based on skip/take query.
                * Currently this is done only for publications, because their dataset is much
                * larger than others.
                */
                
                int skipAmount = 0;
                int takeAmount = 200000;
                int numOfResults = 0;

                do
                {
                    _logger.LogInformation("{EntityType}: Requested {takeAmount} entities from database...", type.Name, takeAmount);
                    var indexModels = await repository.GetChunkAsync(skipAmount: skipAmount, takeAmount: takeAmount).ToListAsync();
                    numOfResults = indexModels.Count;
                    _logger.LogInformation("{EntityType}: ...received {numOfResults} entities", type.Name, numOfResults);
                    
                    if (numOfResults > 0)
                    {
                        _logger.LogInformation("{EntityType}: In-memory operations start", type.Name);
                        foreach (object entity in indexModels) {
                            finalized.Add(repository.PerformInMemoryOperation(entity));
                        }
                        _logger.LogInformation("{EntityType}: In-memory operations complete", type.Name);
                    }
                    skipAmount = skipAmount + takeAmount;
                } while(numOfResults >= takeAmount-1);
            }
            else
            {
                /*
                * Process complete database result at once.
                * Suitable for small result sets.
                */
                _logger.LogInformation("{EntityType}: Requested all entities from database...", type.Name);
                var indexModels = await repository.GetAllAsync().ToListAsync();
                var databaseElapsed = stopWatch.Elapsed;
                _logger.LogInformation("{EntityType}: ..received {DatabaseCount} entities in {ElapsedDatabase}",  type.Name, indexModels.Count, databaseElapsed);
                
                if (indexModels.Count > 0)
                {
                    _logger.LogInformation("{EntityType}: Start in-memory operations", type.Name);
                    finalized = repository.PerformInMemoryOperations(indexModels);
                }
            }
            var inMemoryElapsed = stopWatch.Elapsed;

            if (finalized.Count > 0)
            {
                _logger.LogInformation("{EntityType}: Retrieved and performed in-memory operations to {FinalizedCount} entities in {Elapsed}. Start indexing.",  type.Name, finalized.Count, inMemoryElapsed);
                await _indexService.IndexAsync(indexName, finalized, type);
                var indexingElapsed = stopWatch.Elapsed;
                _logger.LogInformation("{EntityType}: Indexed total of {IndexCount} documents in {ElapsedIndexing}...", type.Name, finalized.Count,  indexingElapsed - inMemoryElapsed);
                _logger.LogInformation("{EntityType}: Index '{IndexName}' recreated successfully in {ElapsedTotal}", type.Name, indexName, stopWatch.Elapsed);
            }
            else
            {
                _logger.LogInformation("{EntityType}: Nothing to index", type.Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{EntityType}: Exception occurred while indexing '{IndexName}' index after {ElapsedException},", type.Name, indexName, stopWatch.Elapsed);
        }
    }
}