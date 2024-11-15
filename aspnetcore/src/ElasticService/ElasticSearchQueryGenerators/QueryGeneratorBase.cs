using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public abstract class QueryGeneratorBase<TIn, TOut> : IQueryGenerator<TIn, TOut> where TOut : class
{
    private readonly IndexNameSettings _configuration;

    protected QueryGeneratorBase(IndexNameSettings configuration)
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
            .Sort(GenerateSortForSearch(searchParameters))
            .Query(GenerateQueryForSearch(searchParameters));
    }

    public Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuerySearchAfter(TIn searchParameters, int pageSize, long? searchAfter)
    {
        var indexName = _configuration.GetIndexNameForType(typeof(TOut));

        if (searchAfter == null) {
            return descriptor => descriptor
                .Index(indexName)
                .Take(pageSize)
                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                .Query(GenerateQueryForSearch(searchParameters));
        }
        else {
            return descriptor => descriptor
                .Index(indexName)
                .Take(pageSize)
                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                .Query(GenerateQueryForSearch(searchParameters))
                .SearchAfter(searchAfter);
        }
    }

    public Func<SearchDescriptor<TOut>, ISearchRequest> GenerateSingleQuery(string id)
    {
        var indexName = _configuration.GetIndexNameForType(typeof(TOut));
        return descriptor => descriptor
            .Index(indexName)
            .Query(GenerateQueryForSingle(id));
    }

    protected abstract Func<QueryContainerDescriptor<TOut>, QueryContainer> GenerateQueryForSearch(TIn parameters);

    protected abstract Func<QueryContainerDescriptor<TOut>,QueryContainer> GenerateQueryForSingle(string id);

    protected abstract Func<SortDescriptor<TOut>, IPromise<IList<ISort>>> GenerateSortForSearch(TIn parameters);
}