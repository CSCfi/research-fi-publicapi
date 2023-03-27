namespace CSC.PublicApi.ElasticService;

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
    /// <param name="modelType"></param>
    /// <returns></returns>
    Task IndexAsync(string indexName, List<object> entities, Type modelType);
}