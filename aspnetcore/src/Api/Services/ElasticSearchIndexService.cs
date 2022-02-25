using Api.Models.Entities;
using Nest;

namespace Api.Services
{
    public class ElasticSearchIndexService : IElasticSearchIndexService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchIndexService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task IndexAsync<T>(string indexName, IReadOnlyCollection<T> entities) where T : class
        {
            if ((await _elasticClient.Indices.ExistsAsync(indexName)).Exists)
            {
                var deleteResponse = await _elasticClient.Indices.DeleteAsync(indexName);
                if(!deleteResponse.IsValid)
                {
                    throw new InvalidOperationException($"Deleting index {indexName} failed.", deleteResponse.OriginalException);
                }
            }

            var createResponse = await _elasticClient.Indices.CreateAsync(indexName, x =>
                x.Map<T>(x => x.AutoMap(maxRecursion: 1)));

            var valid = createResponse.IsValid;

            if (!createResponse.IsValid)
            {
                throw new InvalidOperationException($"Creating index {indexName} failed.", createResponse.OriginalException);
            }

            var bulkResponse = await _elasticClient.BulkAsync(b => b
                .Index(indexName)
                .IndexMany(entities));

            if (!bulkResponse.IsValid)
            {
                throw new InvalidOperationException($"Indexing entities to {indexName} failed.", bulkResponse.OriginalException);
            }

            await _elasticClient.Cluster
                    .HealthAsync(selector: s => s
                        .WaitForStatus(Elasticsearch.Net.WaitForStatus.Yellow)
                        .WaitForActiveShards("1")
                        .Index(indexName)
                        );

        }

    }
}
