using System.Collections.ObjectModel;
using System.Linq;
using ToDo.DataAccess.DataModels.Entities;
using ToDoUICore.MVVMHelper;

namespace ToDo.Core.ViewModel
{
    public class TopicViewModel : BaseViewModel
    {
        private string title;
        private ObservableCollection<ToDoViewModel> toDo;

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public int Id { get; set; }

        public ObservableCollection<ToDoViewModel> ToDo
        {
            get => toDo;
            set => SetProperty(ref toDo, value);
        }

        public string Color { get; set; }
        public ItemStatus Status { get; set; }

        public static TopicViewModel From(TopicEntity topic)
        {
            if (topic == null)
                return null;

            return new TopicViewModel()
            {
                Id = topic.Id,
                Title = topic.Name,
                Color = topic.Color,
                Status = topic.Status,
                ToDo = topic.TodoList == null ?
                        new ObservableCollection<ToDoViewModel>() :
                        new ObservableCollection<ToDoViewModel>(topic.TodoList.Select(x => ToDoViewModel.From(x))),
            };
        }
    }
}