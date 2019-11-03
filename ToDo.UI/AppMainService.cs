using System.Threading.Tasks;
using ToDo.Core.Mock;
using ToDo.DataAccess;
using TODO;
using ToDoUICore.AppService;

namespace ToDoUICore
{
    public class AppMainService
    {
        public async Task<bool> InitilizeAsync(App app)
        {
            await InitDemoData();
            return true;
        }

        private async Task<bool> InitDemoData()
        {
            await Seed.SeedInitialDataAsync(await DependencyServiceSingleton.Instance.GetAsync<TodoDbContext>());
            return true;
        }
    }
}