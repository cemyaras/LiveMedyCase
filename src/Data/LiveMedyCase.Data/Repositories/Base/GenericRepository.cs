using System.Linq.Expressions;
using LiveMedyCase.Core.Entities;
using LiveMedyCase.Core.Repositories;
using LiveMedyCase.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LiveMedyCase.Data.Repositories.Base
{
	public class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TKey : struct
    {
        public AppDbContext Context { get; set; }
        protected DbSet<TEntity> Entities { get; set; }

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            Entities = Context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Entities.FirstOrDefaultAsync(filter);
        }
        public virtual async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            var list = filter != null
                ? Entities.Where(filter)
                : Entities;

            return await list.ToListAsync();
        }

        public virtual async Task<bool> CreateAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
            return await SaveAsync();
        }
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return await SaveAsync();
        }
        public virtual async Task<bool> RemoveAsync(TEntity entity)
        {
            Context.Remove(entity);
            return await SaveAsync();
        }

        protected async Task<bool> SaveAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }
}
