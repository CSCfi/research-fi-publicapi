namespace CSC.PublicApi.DataAccess.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<bool> AddAsync(T entity);
    Task<T> GetAsync(Guid id);
    IQueryable<T> GetAll();

}