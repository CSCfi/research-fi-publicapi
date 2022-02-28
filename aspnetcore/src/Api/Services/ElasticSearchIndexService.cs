using Api.Models.Entities;
using Nest;

namespace Api.Services
{
    public class ElasticSearchIndexService : IElasticSearchIndexService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticSearchIndexService> _logger;

        public ElasticSearchIndexService(IElasticClient elasticClient, ILogger<ElasticSearchIndexService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task IndexAsync<T>(string indexName, List<T> entities) where T : class
        {
            // TODO: use the alias pattern (with _v1, _v2) to reduce downtime.
            if ((await _elasticClient.Indices.ExistsAsync(indexName)).Exists)
            {
                _logger.LogInformation("Index {indexName} exists already.", indexName);

                var deleteResponse = await _elasticClient.Indices.DeleteAsync(indexName);
                if(!deleteResponse.IsValid)
                {
                    throw new InvalidOperationException($"Deleting index {indexName} failed.", deleteResponse.OriginalException);
                }

                _logger.LogInformation("Index {indexName} deleted.", indexName);
            }

            var createResponse = await _elasticClient.Indices.CreateAsync(indexName, x =>
                x.Map<T>(x => x.AutoMap(maxRecursion: 1)));

            var valid = createResponse.IsValid;

            if (!createResponse.IsValid)
            {
                throw new InvalidOperationException($"Creating index {indexName} failed.", createResponse.OriginalException);
            }

            _logger.LogInformation("Index {indexName} created.", indexName);

            var documentBatches = new List<List<T>>();
            const int batchSize = 5;

            for (var docIndex = 0; docIndex < entities.Count; docIndex += batchSize)
            {
                documentBatches.Add(entities.GetRange(docIndex, Math.Min(batchSize, entities.Count - docIndex)));
            }

            foreach (var batchToIndex in documentBatches)
            {
                var indexBatchResponse = await _elasticClient.BulkAsync(b => b
                    .Index(indexName)
                    .IndexMany(batchToIndex));

                if (!indexBatchResponse.IsValid)
                {
                    throw new InvalidOperationException($"Indexing entities to {indexName} failed.", indexBatchResponse.OriginalException);
                }
                _logger.LogInformation("Indexed {batchSize} documents to {indexName}.", batchToIndex.Count, indexName);
            }

            await _elasticClient.Cluster
                    .HealthAsync(selector: s => s
                        .WaitForStatus(Elasticsearch.Net.WaitForStatus.Yellow)
                        .WaitForActiveShards("1")
                        .Index(indexName)
                        );

            _logger.LogInformation("Indexing to {indexName} complete.", indexName);

        }

    }
}
