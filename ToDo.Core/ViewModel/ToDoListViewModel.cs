using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDo.IU.Core.Enums;
using ToDoUICore.MVVMHelper;

namespace ToDo.Core.ViewModel
{
    public class ToDoListViewModel : BaseViewModel
    {
        private TopicViewModel _selectedTopic;
        private ObservableCollection<TopicViewModel> _topics;
        private ToDoViewModel _selectedToDo;
        private bool isFilterWindowsOpened;
        private bool filterComleted;
        private bool filterNotCompleted;
        private bool filterExpired;
        private bool filterToday;
        private OrderType orderBy = OrderType.CreatedAt;
        private string searchQuery;
        private string topicName;
        private bool isAddTopicFormVisible;

        public bool IsAddTopicFromVisible
        {
            get => isAddTopicFormVisible;
            set => SetProperty(ref isAddTopicFormVisible, value);
        }

        public string NewTopicName
        {
            get => topicName;
            set => SetProperty(ref topicName, value);
        }

        public ToDoViewModel SelectedToDo
        {
            get => _selectedToDo;
            set => SetProperty(ref _selectedToDo, value);
        }

        public ObservableCollection<TopicViewModel> Topics
        {
            get => _topics;
            set => SetProperty(ref _topics, value);
        }

        public TopicViewModel SelectedMenu
        {
            get => _selectedTopic;
            set => SetProperty(ref _selectedTopic, value);
        }

        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                SetProperty(ref searchQuery, value);
                if (!string.IsNullOrEmpty(searchQuery) && searchQuery.Length >= 2)
                {
                    ApplyFilter?.Execute(null);
                }
            }
        }

        public bool IsFilterWindowsOpened
        {
            get => isFilterWindowsOpened;
            set => SetProperty(ref isFilterWindowsOpened, value);
        }

        public bool FilterComleted
        {
            get => filterComleted;
            set => SetProperty(ref filterComleted, value);
        }

        public bool FilterToday
        {
            get => filterToday;
            set => SetProperty(ref filterToday, value);
        }

        public bool FilterNotCompleted
        {
            get => filterNotCompleted;
            set => SetProperty(ref filterNotCompleted, value);
        }

        public bool FilterExpired
        {
            get => filterExpired;
            set => SetProperty(ref filterExpired, value);
        }

        public OrderType OrderBy
        {
            get => orderBy;
            set => SetProperty(ref orderBy, value);
        }

        public ICommand ApplyFilter { get; set; }
        public ICommand ToDoSelected { get; set; }
        public ICommand ToDoArchived { get; set; }
        public ICommand ChangeFilterWindowsOpened { get; set; }
        public ICommand CloseFilterWindowsOpened { get; set; }
        public ICommand AddTodo { get; set; }
        public ICommand AddNewTopic { get; set; }
        public ICommand CancelTopic { get; set; }
    }
}