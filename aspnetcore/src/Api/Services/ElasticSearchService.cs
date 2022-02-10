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

        public IReadOnlyCollection<TOut> Search(TIn parameters)
        {
            var query = _queryGenerator.GenerateQuery(parameters);

            var searchResult = _elasticClient.Search(query);
            return searchResult.Documents;
        }
    }
}
