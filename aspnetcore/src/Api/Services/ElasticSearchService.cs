using Nest;

namespace Api.Services
{
    public class ElasticSearchService<T> : ISearchService<T> where T : class
    {
        private readonly IElasticClient _elasticClient;
        private readonly IQueryGenerator<T> _queryGenerator;

        public ElasticSearchService(
            IElasticClient elasticClient,
            IQueryGenerator<T> queryGenerator)
        {
            _elasticClient = elasticClient;
            _queryGenerator = queryGenerator;
        }

        public IReadOnlyCollection<T> Search(string searchText)
        {
            var query = _queryGenerator.GenerateQuery(searchText);

            var searchResult = _elasticClient.Search(query);
            return searchResult.Documents;
        }
    }
}
