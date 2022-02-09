namespace Api.Services
{
    public interface ISearchService<T> where T : class
    {
        IReadOnlyCollection<T> Search(string searchText);
    }
}
