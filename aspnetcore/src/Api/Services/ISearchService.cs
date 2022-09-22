using Api.Models;

namespace Api.Services
{
    public interface ISearchService<TIn, TOut> where TOut : class
    {
        Task<SearchResult<TOut>> Search(TIn searchParameters, int pageNumber, int pageSize);
    }
}
