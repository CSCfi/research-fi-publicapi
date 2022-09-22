using Api.Models;
using Api.Services.ElasticSearchQueryGenerators;
using Nest;

namespace Api.Services
{
    public class ElasticSearchService<TIn,TOut> : ISearchService<TIn,TOut> where TOut : class
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

        public async Task<SearchResult<TOut>> Search(TIn parameters, int pageNumber, int pageSize)
        {
            var query = _queryGenerator.GenerateQuery(parameters, pageNumber, pageSize);

            var searchResult = await _elasticClient.SearchAsync(query);

            return new SearchResult<TOut>(searchResult.Documents, pageNumber, pageSize, searchResult.HitsMetadata?.Total.Value);
        }
    }
}
