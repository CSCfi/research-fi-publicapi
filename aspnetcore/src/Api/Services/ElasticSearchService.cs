using Nest;



namespace Api.Services
{
    public class ElasticSearchService : ISearchService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public IReadOnlyCollection<T> Search<T>(string searchText) where T : class
        {
            var searchResult = _elasticClient.Search<T>(t => t
                .Index("publication")
                .Query(q =>q
                    .MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("publicationName")
                        .Query(searchText))));
            return searchResult.Documents;
                    
        }

    }
}
