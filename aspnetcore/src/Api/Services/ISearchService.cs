namespace Api.Services
{
    public interface ISearchService
    {
        IEnumerable<T> Search<T>() where T : new();
    }
}
