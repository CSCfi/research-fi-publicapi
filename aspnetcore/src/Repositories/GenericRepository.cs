using CSC.PublicApi.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DbSet<T> DbSet;

    protected GenericRepository(ApiDbContext context)
    {
        DbSet = context.Set<T>();
    }

    public async Task<bool> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return true;
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public IQueryable<T> GetAll()
    {
        return DbSet;
    }
}