using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CSC.PublicApi.Repositories;

/// <summary>
/// Base class for IndexRepositories. Provides common logic for every IndexRepository.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class IndexRepositoryBase<T> : IIndexRepository<T> where T : class
{
    protected readonly IMemoryCache MemoryCache;
    
    protected IndexRepositoryBase(IMemoryCache memoryCache)
    {
        MemoryCache = memoryCache;
    }

    /// <summary>
    /// Method which every IndexRepository must override, provides every T as Queryable.
    /// </summary>
    /// <returns></returns>
    protected abstract IQueryable<T> GetAll();

    /// <summary>
    /// Method which every IndexRepository must override, provides every T as Queryable.
    /// </summary>
    /// <returns></returns>
    protected abstract IQueryable<T> GetChunk(int skipAmount, int takeAmount);
    
    Type IIndexRepository.ModelType => typeof(T);

    IQueryable<T> IIndexRepository<T>.GetAll() => GetAll();

    IQueryable<T> IIndexRepository<T>.GetChunk(int skipAmount, int takeAmount) => GetChunk(skipAmount, takeAmount);

    IAsyncEnumerable<object> IIndexRepository.GetAllAsync() => GetAll().AsAsyncEnumerable();

    IAsyncEnumerable<object> IIndexRepository.GetChunkAsync(int skipAmount, int takeAmount) => GetChunk(skipAmount, takeAmount).AsAsyncEnumerable();

    /// <summary>
    /// If the data type needs special data manipulations after data has been fetched,
    /// this can be overriden in derived classes.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual List<object> PerformInMemoryOperations(List<object> entities)
    {
        return entities;
    }

    /// <summary>
    /// If the data type needs special data manipulations after data has been fetched,
    /// this can be overriden in derived classes.
    /// Single entity version.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual object PerformInMemoryOperation(object entity)
    {
        return entity;
    }
    
    protected Service.Models.Organization.Organization? GetOrganization(int organizationId)
    {
        return MemoryCache.TryGetValue<CSC.PublicApi.Service.Models.Organization.Organization>(MemoryCacheKeys.OrganizationById(organizationId), out var organization) ? organization : null;
    }
}