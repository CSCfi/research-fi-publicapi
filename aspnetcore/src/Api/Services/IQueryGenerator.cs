using Nest;

namespace Api.Services
{
    public interface IQueryGenerator<T> where T : class
    {
        Func<SearchDescriptor<T>, ISearchRequest> GenerateQuery(string searchText);
    }
}
