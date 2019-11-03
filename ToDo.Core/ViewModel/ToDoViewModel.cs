using ToDo.DataAccess.DataModels.Entities;
using ToDoUICore.MVVMHelper;

namespace ToDo.Core.ViewModel
{
    public class ToDoViewModel : BaseViewModel
    {
        private string title;
        private NoteStatus status;
        public int Id { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public NoteStatus Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public string Color { get; private set; }

        public static ToDoViewModel From(TodoEntity task)
        {
            if (task == null)
                return null;

            return new ToDoViewModel()
            {
                Id = task.Id,
                Status = task.Status,
                Title = task.Title,
                Color = task.Color,
            };
        }
    }
}