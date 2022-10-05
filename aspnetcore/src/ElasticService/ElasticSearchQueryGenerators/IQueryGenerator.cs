using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public interface IQueryGenerator<TIn, TOut> where TOut : class
{
    Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuery(TIn searchParameters, int pageNumber, int pageSize);
}