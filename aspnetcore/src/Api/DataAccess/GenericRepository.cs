using Api.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApiDbContext context;
        internal DbSet<T> dbSet;

        public GenericRepository(ApiDbContext context)
        {
            this.context = context;
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
    }
}
