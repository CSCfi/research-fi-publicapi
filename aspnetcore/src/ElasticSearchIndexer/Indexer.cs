using Api.Configuration;
using Api.DataAccess.Repositories;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace ElasticSearchIndexer
{
    public class Indexer
    {
        private readonly ILogger<Indexer> _logger;
        private readonly IElasticSearchIndexService _indexService;
        private readonly IConfiguration _configuration;
        private readonly IndexNameSettings _indexNameSettings;
        private readonly IEnumerable<IIndexRepository> _indexRepositories;

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
            Stopwatch stopWatch = new();
            stopWatch.Start();
            _logger.LogInformation("Starting indexing.. {stopWatch}", stopWatch.Elapsed);

            var elasticLog = $"Using ElasticSearch '{_configuration["ELASTICSEARCH:URL"]}'";
            elasticLog += _configuration["ELASTICSEARCH:USERNAME"] != null || _configuration["ELASTICSEARCH:PASSWORD"] != null
                ? " with basic authentication."
                : ".";
            _logger.LogInformation(elasticLog);

            var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();

            var indexingResults = new List<(string IndexName, bool Success, TimeSpan Elapsed)>();

            foreach (var indexNameAndType in configuredTypesAndIndexNames)
            {
                var indexName = indexNameAndType.Key;
                var modelType = indexNameAndType.Value;
                var repositoryForType = _indexRepositories.Single(repo => repo.ModelType == modelType);

                var indexingResult = await IndexEntities(indexName, repositoryForType, modelType);

                indexingResults.Add(indexingResult);

            }

            var summaryText = new StringBuilder();
            summaryText.AppendLine($"All indexing done. Summary:");
            foreach (var (IndexName, Success, Elapsed) in indexingResults)
            {
                summaryText.AppendLine($"{IndexName}: {(Success ? "OK" : "Error")} - {Elapsed}");
            }
            summaryText.AppendLine($"ALL: {indexingResults.Count(r => r.Success)} Ok, {indexingResults.Count(r => !r.Success)} failed. - {stopWatch.Elapsed.ToString("c")}");
            _logger.LogInformation(summaryText.ToString());
            stopWatch.Stop();
        }

        private async Task<(string IndexName, bool Success, TimeSpan Elapsed)> IndexEntities(
            string indexName,
            IIndexRepository repository,
            Type type)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();

            try
            {

                _logger.LogInformation("Getting '{entityType}' entities from the database.", type.Name);

                var indexModels = await repository.GetAllAsync().ToListAsync();
                _logger.LogInformation("Got {count} '{entityType}' entities from the database. {stopWatch}", indexModels.Count, type.Name, stopWatch.Elapsed);

                _logger.LogInformation("Indexing '{indexName}' to ElasticSearch..", indexName);

                await _indexService.IndexAsync(indexName, indexModels, type);
                _logger.LogInformation("Index '{indexName}' created. {stopWatch}", indexName, stopWatch.Elapsed);

                return (indexName, true, stopWatch.Elapsed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while indexing '{indexName}' {stopWatch},", indexName, stopWatch.Elapsed);
                return (indexName, false, stopWatch.Elapsed);
            }
        }
    }
}
