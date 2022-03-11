using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    public abstract class QueryGeneratorBase<TIn, TOut> : IQueryGenerator<TIn, TOut> where TOut : class
    {
        protected readonly IConfiguration _configuration;

        public QueryGeneratorBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuery(TIn searchParameters)
        {
            var indexName = GetIndexNameForModelType(typeof(TOut));
            return GenerateQueryForIndex(searchParameters, indexName);
        }

        protected abstract Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQueryForIndex(TIn searchParameters, string indexName);

        private string GetIndexNameForModelType(Type type)
        {
            var configKey = $"IndexNames:{type.Name}";
            return _configuration[configKey] ?? throw new InvalidOperationException($"Configuration value missing for '{configKey}'.");
        }
    }
}
