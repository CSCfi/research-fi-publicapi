namespace Api.Services
{
    public interface ISearchService
    {
        IReadOnlyCollection<T> Search<T>(string searchText) where T : class;
    }
}
