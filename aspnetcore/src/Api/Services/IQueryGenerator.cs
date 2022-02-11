using Nest;

namespace Api.Services
{
    public interface IQueryGenerator<TIn, TOut> where TOut : class
    {
        Func<SearchDescriptor<TOut>, ISearchRequest> GenerateQuery(TIn searchParameters);
    }
}
