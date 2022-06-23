using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    /// <summary>
    /// Base class for IndexRepositories. Provides common logic for every IndexRepository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IndexRepositoryBase<T> : IIndexRepository<T> where T : class
    {
        /// <summary>
        /// Method which every IndexRepository must override, provides every T as Queryable.
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<T> GetAll();
        Type IIndexRepository.ModelType => typeof(T);

        IQueryable<T> IIndexRepository<T>.GetAll() => GetAll();

        IAsyncEnumerable<object> IIndexRepository.GetAllAsync() => GetAll().AsAsyncEnumerable();

        /// <summary>
        /// If the data type needs special data manipulations after data has been fetched,
        /// this can be overriden in derived classes.
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public virtual List<object> PerformInMemoryOperations(List<object> objects)
        {
            return objects;
        }
    }
}