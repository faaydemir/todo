using System.Threading.Tasks;
using ToDo.Core.Model;

namespace ToDo.Core.Services
{
    public interface IAppStateService
    {
        Task<User> GetCurrentUser();

        void SetCurrentUser(User user);

        void RemoveCurrentUser();
    }
}