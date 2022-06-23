using Api.Configuration;
using Api.DataAccess.Repositories;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ElasticSearchIndexer
{
    public class Indexer
    {
        private readonly ILogger<Indexer> _logger;
        private readonly IElasticSearchIndexService _indexService;
        private readonly IConfiguration _configuration;
        private readonly IndexNameSettings _indexNameSettings;
        private readonly IEnumerable<IIndexRepository> _indexRepositories;
        private readonly Stopwatch _stopWatch = new();

        public Indexer(
            ILogger<Indexer> logger,
            IElasticSearchIndexService indexService,
            IConfiguration configuration,
            IndexNameSettings indexNameSettings,
            IEnumerable<IIndexRepository> indexRepositories
            )
        {
            _logger = logger;
            _indexService = indexService;
            _configuration = configuration;
            _indexNameSettings = indexNameSettings;
            _indexRepositories = indexRepositories;
        }

        public async Task Start()
        {
            _stopWatch.Start();
            _logger.LogInformation("Starting indexing.. {stopWatch}", _stopWatch.Elapsed);

            var elasticLog = $"Using ElasticSearch '{_configuration["ELASTICSEARCH:URL"]}'";
            elasticLog += _configuration["ELASTICSEARCH:USERNAME"] != null || _configuration["ELASTICSEARCH:PASSWORD"] != null
                ? " with basic authentication."
                : ".";
            _logger.LogInformation(elasticLog);

            var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();

            foreach (var indexNameAndType in configuredTypesAndIndexNames)
            {
                var indexName = indexNameAndType.Key;
                var modelType = indexNameAndType.Value;
                var repositoryForType = _indexRepositories.Single(repo => repo.ModelType == modelType);

                await IndexEntities(indexName, repositoryForType, modelType);
            }

            _logger.LogInformation("All indexing done. {stopWatch}", _stopWatch.Elapsed);
            _stopWatch.Stop();
        }

        private async Task IndexEntities(
            string indexName,
            IIndexRepository repository,
            Type type
        )
        {
            try
            {
                
                _logger.LogInformation("Getting '{entityType}' entities from the database. {stopWatch}", type.Name, _stopWatch.Elapsed);

                var indexModels = await repository.GetAllAsync().ToListAsync();
                _logger.LogInformation("Got {count} '{entityType}' entities from the database. {stopWatch}", indexModels.Count, type.Name, _stopWatch.Elapsed);
                var finalized = repository.PerformInMemoryOperations(indexModels);

                _logger.LogInformation("Indexing '{indexName}'.. {stopWatch}", indexName, _stopWatch.Elapsed);

                await _indexService.IndexAsync(indexName, finalized, type);
                _logger.LogInformation("Index '{indexName}' created. {stopWatch}", indexName, _stopWatch.Elapsed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while indexing '{indexName}' {stopWatch},", indexName, _stopWatch.Elapsed);
            }
        }
    }
}
