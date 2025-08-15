using System.Linq.Expressions;

namespace ProductAPI.Infrastructure.Repositories
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task AddAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(Guid id);

        void Remove(T entity);

        void Update(T entity);

        Task<bool> SaveChangesAsync();

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    }
}
