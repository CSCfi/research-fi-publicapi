using Api.Models;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    public interface IQueryGenerator<TIn, TOut> where TOut : class
    {
        Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuery(TIn searchParameters, int pageNumber, int pageSize);
    }
}
