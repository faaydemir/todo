using System.Collections.ObjectModel;
using ToDo.Core.ViewModel;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.UI
{
    public class DesingTimeToDoListViewModel : ToDoListViewModel
    {
        public DesingTimeToDoListViewModel()
        {
            Topics = new ObservableCollection<TopicViewModel>()
                {
                    new TopicViewModel()
                    {
                        Title="test menu",
                        Color ="#BCB0C3",
                        Status= ItemStatus.Active,
                        ToDo =new ObservableCollection<ToDoViewModel>()
                        {
                            new ToDoViewModel()
                            {
                                Title="test todo",
                                Status= NoteStatus.Active,
                            }
                        }
                    }
                };
        }
    }
}