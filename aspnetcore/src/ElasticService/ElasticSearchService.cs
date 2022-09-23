using CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;
using Nest;

namespace CSC.PublicApi.ElasticService;

public class ElasticSearchService<TIn, TOut> : ISearchService<TIn, TOut> where TOut : class
{
    private readonly IElasticClient _elasticClient;
    private readonly IQueryGenerator<TIn, TOut> _queryGenerator;

    public ElasticSearchService(
        IElasticClient elasticClient,
        IQueryGenerator<TIn, TOut> queryGenerator)
    {
        _elasticClient = elasticClient;
        _queryGenerator = queryGenerator;
    }

    public async Task<(IEnumerable<TOut>, SearchResult)> Search(TIn parameters, int pageNumber, int pageSize)
    {
        var query = _queryGenerator.GenerateQuery(parameters, pageNumber, pageSize);

        var searchResult = await _elasticClient.SearchAsync(query);

        return (searchResult.Documents, new SearchResult(pageNumber, pageSize, searchResult.HitsMetadata?.Total.Value));
    }
}