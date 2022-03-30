namespace Api.DataAccess.Repositories
{
    /// <summary>
    /// Responsible for fetching entities from database and mapping them to api models which are indexed to ElasticSearch.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIndexRepository<T>
    {
        IAsyncEnumerable<T> GetAllAsync();
    }
}