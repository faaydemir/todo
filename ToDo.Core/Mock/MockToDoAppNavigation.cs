using System.Threading.Tasks;
using ToDo.Core.Services;

namespace Todo.App.Test
{
    public class MockToDoAppNavigation : IToDoAppNavigation
    {
        public event ToDoChangedEventHandler ToDoChangedEvent;

        public Task<bool> OpenAuthPage()
        {
            return Task.FromResult(true);
        }

        public Task<bool> OpenToDoDetailAsync(int id)
        {
            ToDoChangedEvent?.Invoke(null, new ToDoChangedEventArgs() { TodoId = id });
            return Task.FromResult(true);
        }

        public Task<bool> OpenToDoDetailForNew(int id)
        {
            ToDoChangedEvent?.Invoke(null, new ToDoChangedEventArgs() { TopicId = id });
            return Task.FromResult(true);
        }

        public Task<bool> OpenTodoPage()
        {
            return Task.FromResult(true);
        }
    }
}