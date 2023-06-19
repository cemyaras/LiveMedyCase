using System.Linq.Expressions;
using LiveMedyCase.Core.Entities;

namespace LiveMedyCase.Core.Repositories
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct
    {
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> RemoveAsync(TEntity entity);
    }
}
