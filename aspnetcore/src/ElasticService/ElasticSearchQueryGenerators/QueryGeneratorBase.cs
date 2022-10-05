using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public abstract class QueryGeneratorBase<TIn, TOut> : IQueryGenerator<TIn, TOut> where TOut : class
{
    protected readonly IndexNameSettings _configuration;

    public QueryGeneratorBase(IndexNameSettings configuration)
    {
        _configuration = configuration;
    }

    public Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuery(TIn searchParameters, int pageNumber, int pageSize)
    {
        var indexName = _configuration.GetIndexNameForType(typeof(TOut));
        return descriptor => descriptor
            .Index(indexName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Query(GenerateQueryForIndex(searchParameters));
    }

    protected abstract Func<QueryContainerDescriptor<TOut>, QueryContainer> GenerateQueryForIndex(TIn searchParameters);
}