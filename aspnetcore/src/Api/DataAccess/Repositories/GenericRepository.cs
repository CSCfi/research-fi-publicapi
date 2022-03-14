using Api.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbSet<T> dbSet;

        public GenericRepository(ApiDbContext context)
        {
            dbSet = context.Set<T>();
        }


        public async Task<bool> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }


        public async Task<T> GetAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public IAsyncEnumerable<T> GetAllAsync()
        {
            return dbSet.AsAsyncEnumerable();
        }
    }
}
