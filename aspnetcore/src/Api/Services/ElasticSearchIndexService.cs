using Nest;

namespace Api.Services
{
    public class ElasticSearchIndexService : IElasticSearchIndexService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticSearchIndexService> _logger;
        
        private const int batchSize = 1000;

        public ElasticSearchIndexService(IElasticClient elasticClient, ILogger<ElasticSearchIndexService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task IndexAsync(string indexName, List<object> entities, Type modelType)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentException("IndexName is null or empty.", nameof(indexName));
            }

            var (indexToCreate, indexToDelete) = await GetIndexNames(indexName);

            // Create new index with _vx prefix.
            await CreateIndex(indexToCreate, modelType);

            // Add entities to the new index.
            await IndexEntities(indexToCreate, entities);

            // Wait for new index to be operational.
            await _elasticClient.Cluster
                .HealthAsync(selector: s => s
                    .WaitForStatus(Elasticsearch.Net.WaitForStatus.Yellow)
                    .WaitForActiveShards("1")
                    .Index(indexToCreate));

            // Add new alias from index_new to index
            await _elasticClient.Indices.BulkAliasAsync(r => r
                // Remove alias "index_old => index"
                .Remove(remove => remove.Alias(indexName).Index("*"))
                // Add alias "index_new => index"
                .Add(add => add.Alias(indexName).Index(indexToCreate)));

            // Delete the old index if it exists.
            await _elasticClient.Indices.DeleteAsync(indexToDelete, d => d.RequestConfiguration(x => x.AllowedStatusCodes(404)));
    
            _logger.LogInformation("Indexing to {indexName} complete.", indexName);

        }

        private async Task<(string indexToCreate, string indexToDelete)> GetIndexNames(string indexName)
        {
            var indexNameV1 = $"{indexName}_v1";
            var indexNameV2 = $"{indexName}_v2";

            var v1IndexExists = (await _elasticClient.Indices.ExistsAsync(indexNameV1)).Exists;

            string indexToDelete;
            string indexToCreate;

            if (!v1IndexExists)
            {
                indexToDelete = indexNameV2;
                indexToCreate = indexNameV1;
            }
            else
            {
                indexToDelete = indexNameV1;
                indexToCreate = indexNameV2;
            }

            return (indexToCreate, indexToDelete);
        }

        private async Task IndexEntities<T>(string indexName, List<T> entities) where T : class
        {
            // Split entities into batches to avoid one big request.
            var documentBatches = new List<List<T>>();
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
                    var errorMessage = $"Indexing entities to {indexName} failed.";
                    _logger.LogError(indexBatchResponse.OriginalException, errorMessage);
                    throw new InvalidOperationException(errorMessage, indexBatchResponse.OriginalException);
                }
                _logger.LogInformation("Indexed {batchSize} documents to {indexName}.", batchToIndex.Count, indexName);
            }
        }

        /// <summary>
        /// Creates index with the given name.
        /// If the index exists already, it will be deleted first.
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task CreateIndex(string indexName, Type type)
        {
            await _elasticClient.Indices.DeleteAsync(indexName, d => d.RequestConfiguration(x => x.AllowedStatusCodes(404)));


            var createResponse = await _elasticClient.Indices.CreateAsync(indexName, x => x.Map(x => x.AutoMap(type, maxRecursion: 1)));

            if (!createResponse.IsValid)
            {
                throw new InvalidOperationException($"Creating index {indexName} failed.", createResponse.OriginalException);
            }

            _logger.LogInformation("Index {indexName} created.", indexName);
        }

    }
}
