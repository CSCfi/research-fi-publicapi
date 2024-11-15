namespace CSC.PublicApi.ElasticService;

public interface ISearchService<TIn, TOut> where TOut : class
{
    Task<(IEnumerable<TOut>, SearchResult)> Search(TIn searchParameters, int pageNumber, int pageSize);
    Task<(IEnumerable<TOut>, SearchAfterResult)> SearchAfter(TIn searchParameters, int pageSize, long? searchAfter);
    Task<TOut?> GetSingle(string id);
}