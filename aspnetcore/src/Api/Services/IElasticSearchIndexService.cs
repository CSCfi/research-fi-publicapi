namespace Api.Services
{
    /// <summary>
    /// Responsible for creating ElasticSearch indices and indexing documents.
    /// </summary>
    public interface IElasticSearchIndexService
    {
        /// <summary>
        /// Creates a new index with the given name and indexses the given entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task IndexAsync<T>(string indexName, List<T> entities) where T : class;
    }
}
