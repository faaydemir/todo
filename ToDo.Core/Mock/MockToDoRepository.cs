using EFCoreUtils;
using System.Threading.Tasks;
using ToDo.DataAccess;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.Core.Mock
{
    public class MockToDoRepository : MockRepository<TodoEntity>, IToDoRepository
    {
        public MockToDoRepository()
        {
            Entities = MockEntityGenerator.CreateToDo(30);
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