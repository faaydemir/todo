using EntityFrameworkCoreHelper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.DataAccess.Repositories
{
    public class TodoRepository : GenericRepository<TodoEntity>, IToDoRepository
    {
        public TodoRepository(DbContext context) : base(context)
        {
        }

        public async Task<bool> SetTaskStatusAsync(int id, NoteStatus status)
        {
            var item = await GetAsync(id);

            if (item == null)
            {
                return false;
            }
            item.Status = status;

            return await UpdateAsync(item);
        }
    }
}