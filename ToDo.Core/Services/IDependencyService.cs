using System;
using System.Threading.Tasks;

namespace ToDo.Core.Services
{
    public interface IDependencyService
    {
        Task<object> GetAsync(Type type);

        Task<T> GetAsync<T>();
    }
}