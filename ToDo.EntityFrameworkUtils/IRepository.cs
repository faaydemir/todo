using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCoreHelper
{
    public interface IRepository<TEntity, TKey> : IUnitOfWork
    {
        Task<TEntity> GetAsync(TKey id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> DeleteAsync(TKey id);

        Task<bool> UpdateAsync(TEntity entity);

        IQueryable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
    }
}