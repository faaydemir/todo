using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCoreHelper
{
    public class GenericRepository<T> : IRepository<T, int> where T : class, IEntity
    {
        protected DbContext context;
        protected DbSet<T> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual Task<T> GetAsync(int id)
        {
            return dbSet.FindAsync(id).AsTask();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public virtual Task<bool> UpdateAsync(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(true);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            T entityToDelete = await dbSet.FindAsync(id);
            if (entityToDelete == null)
                return false;
            return await DeleteAsync(entityToDelete);
        }

        public virtual Task<bool> DeleteAsync(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query;
        }

        public virtual async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string includeProperties = "")
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
    }
}