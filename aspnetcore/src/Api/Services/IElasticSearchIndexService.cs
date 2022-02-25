namespace Api.Services
{
    /// <summary>
    /// Responsible for creating ElasticSearch indices and indexing documents.
    /// </summary>
    public interface IElasticSearchIndexService
    {
        Task IndexAsync<T>(string indexName, IReadOnlyCollection<T> entities) where T : class;
    }
}
