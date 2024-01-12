namespace CSC.PublicApi.ElasticService;

/// <summary>
/// Responsible for creating ElasticSearch indices and indexing documents.
/// </summary>
public interface IElasticSearchIndexService
{
    /// <summary>
    /// Creates a new index with the given name and indexes the given entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="indexName"></param>
    /// <param name="entities"></param>
    /// <param name="modelType"></param>
    /// <returns></returns>
    Task IndexAsync(string indexName, List<object> entities, Type modelType);

    /// <summary>
    /// Index given entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="indexName"></param>
    /// <param name="entities"></param>
    /// <param name="modelType"></param>
    /// <returns></returns>
    Task IndexChunkAsync(string indexName, List<object> entities, Type modelType);

    /// <summary>
    /// Get name of index to create and name of index to delete.
    /// </summary>
    /// <param name="indexName"></param>
    /// <returns></returns>
    Task<(string indexToCreate, string indexToDelete)> GetIndexNames(string indexName);

    /// <summary>
    /// Creates index with the given name.
    /// If the index exists already, it will be deleted first.
    /// </summary>
    /// <param name="indexName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    Task CreateIndex(string indexName, Type type);

    /// <summary>
    /// Switch index
    /// </summary>
    /// <param name="indexName"></param>
    /// <param name="indexToCreate"></param>
    /// <param name="indexToDelete"></param>
    /// <returns></returns>
    Task SwitchIndexes(string indexName, string indexToCreate, string indexToDelete);
}