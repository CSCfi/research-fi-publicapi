using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public interface IQueryGenerator<TIn, TOut> where TOut : class
{
    Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuery(TIn searchParameters, int pageNumber, int pageSize);
    Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuerySearchAfter(TIn searchParameters, int pageSize, long? searchAfter);
    Func<SearchDescriptor<TOut>, ISearchRequest> GenerateSingleQuery(string id);
}