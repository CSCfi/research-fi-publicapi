using CSC.PublicApi.ElasticService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using CSC.PublicApi.Repositories;
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

    private readonly IMemoryCache _organizationCache;

    public Indexer(
        ILogger<Indexer> logger,
        IElasticSearchIndexService indexService,
        IConfiguration configuration,
        IndexNameSettings indexNameSettings,
        IEnumerable<IIndexRepository> indexRepositories, 
        IMemoryCache organizationCache)
    {
        _logger = logger;
        _indexService = indexService;
        _configuration = configuration;
        _indexNameSettings = indexNameSettings;
        _indexRepositories = indexRepositories;
        _organizationCache = organizationCache;
    }

    public async Task Start()
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();
        
        await PopulateOrganizationCache();

        _logger.LogInformation("Starting indexing...");
        _logger.LogInformation("Using ElasticSearch at '{ElasticSearchAddress}'", _configuration["ELASTICSEARCH:URL"]);

        var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();

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
        _logger.LogInformation("Populating Organization cache...");

        var organizationRepository = _indexRepositories.SingleOrDefault(repo => repo.ModelType == typeof(Organization));
        if (organizationRepository != null)
        {
            var organizations = await organizationRepository.GetAllAsync().ToListAsync();
            foreach (Organization organization in organizations)
            {
                _organizationCache.Set(organization.Id, organization);
            }
        }
    }

    private async Task IndexEntities(string indexName,
        IIndexRepository repository,
        Type type)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        try
        {
            _logger.LogDebug("{EntityType}: Recreating '{IndexName}' index...", type.Name, indexName);
            
            var indexModels = await repository.GetAllAsync().ToListAsync();

            var databaseElapsed = stopWatch.Elapsed;
            
            _logger.LogDebug("{EntityType}: Retrieved {DatabaseCount} entities from the database in {ElapsedDatabase}...",  type.Name, indexModels.Count, databaseElapsed);

            var finalized = repository.PerformInMemoryOperations(indexModels);
            
            var inMemoryElapsed = stopWatch.Elapsed;

            _logger.LogDebug("{EntityType}: Performed in-memory operations in {ElapsedInMemory}...",  type.Name, inMemoryElapsed - databaseElapsed);

            await _indexService.IndexAsync(indexName, finalized, type);
            
            var indexingElapsed = stopWatch.Elapsed;
            
            _logger.LogDebug("{EntityType}: Indexed {IndexCount} entities in {ElapsedIndexing}...", type.Name, indexModels.Count,  indexingElapsed - inMemoryElapsed);

            _logger.LogInformation("{EntityType}: Index '{IndexName}' recreated successfully with {IndexCount} entities in {ElapsedTotal}", type.Name, indexName, indexModels.Count, stopWatch.Elapsed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{EntityType}: Exception occurred while indexing '{IndexName}' index after {ElapsedException},", type.Name, indexName, stopWatch.Elapsed);
        }
    }
}