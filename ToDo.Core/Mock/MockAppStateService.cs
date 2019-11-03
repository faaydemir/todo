using System.Threading.Tasks;
using ToDo.Core.Model;
using ToDo.Core.Services;

namespace Todo.App.Test
{
    public class MockAppStateService : IAppStateService
    {
        private User CurrentUser;

        public MockAppStateService()
        {
            CurrentUser = new User()
            {
                Mail = "test@test.com",
                Name = "test",
                UserBagId = 1,
                Id = 1,
            };
        }

        public Task<User> GetCurrentUser()
        {
            return Task.FromResult(CurrentUser);
        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public void RemoveCurrentUser()
        {
            SetCurrentUser(null);
        }
    }
}