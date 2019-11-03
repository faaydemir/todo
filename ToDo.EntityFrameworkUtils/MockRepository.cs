using EntityFrameworkCoreHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCoreUtils
{
    public class MockRepository<T> : IRepository<T, int> where T : class, IEntity
    {
        protected IList<T> Entities;
        protected int idIndex;

        public MockRepository()
        {
            Entities = new List<T>();
            idIndex = 0;
        }

        public Task<T> AddAsync(T entity)
        {
            entity.Id = ++idIndex;
            Entities.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<bool> DeleteAsync(T entity)
        {
            var isRemoved = Entities.Remove(entity);

            return Task.FromResult(isRemoved);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await GetAsync(id);

            if (item == null)
            {
                return false;
            }
            else
            {
                return await DeleteAsync(item);
            }
        }

        public IQueryable<T> GetAll()
        {
            return Entities.AsQueryable();
        }

        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            return Task.FromResult(Entities.AsEnumerable());
        }

        public Task<T> GetAsync(int id)
        {
            return Task.FromResult(Entities.FirstOrDefault(x => x.Id == id));
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var isDeleted = await DeleteAsync(entity);
            if (isDeleted)
            {
                Entities.Add(entity);
            }

            return isDeleted;
        }
    }
}