namespace Api.Services
{
    public class DummySearchService : ISearchService
    {
        public IEnumerable<T> Search<T>() where T : new()
        {
            return Enumerable.Range(1, 5).Select(index => new T()).ToArray();
        }
    }
}
