using EntityFrameworkCoreHelper;
using System.Threading.Tasks;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.DataAccess
{
    public interface IToDoRepository : IRepository<TodoEntity, int>
    {
        Task<bool> SetTaskStatusAsync(int id, NoteStatus status);
    }
}