using System;
using System.Threading.Tasks;

namespace ToDo.Core.Services
{
    public class ToDoChangedEventArgs : EventArgs
    {
        public int? TodoId { get; set; }
        public int? TopicId { get; set; }
    }

    public delegate void ToDoChangedEventHandler(object sender, ToDoChangedEventArgs eventArgs);

    public interface IToDoAppNavigation
    {
        event ToDoChangedEventHandler ToDoChangedEvent;

        Task<bool> OpenAuthPage();

        Task<bool> OpenTodoPage();

        Task<bool> OpenToDoDetailAsync(int id);

        Task<bool> OpenToDoDetailForNew(int id);
    }
}