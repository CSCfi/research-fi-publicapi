using Api.Configuration;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    public abstract class QueryGeneratorBase<TIn, TOut> : IQueryGenerator<TIn, TOut> where TOut : class
    {
        protected readonly IndexNameSettings _configuration;

        public QueryGeneratorBase(IndexNameSettings configuration)
        {
            _configuration = configuration;
        }

        public Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuery(TIn searchParameters)
        {
            var indexName = _configuration.GetIndexNameForType(typeof(TOut));
            return GenerateQueryForIndex(searchParameters, indexName);
        }

        protected abstract Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQueryForIndex(TIn searchParameters, string indexName);
    }
}
