using System.Windows.Controls;
using ToDo.Core.ViewModel;

namespace ToDo.UI.Views
{
    /// <summary>
    /// Interaction logic for ToDoList.xaml
    /// </summary>
    public partial class ToDoList : UserControlBase
    {
        public ToDoList()
        {
            InitializeComponent();
            BindAsync<ToDoListBinder>();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Model != null && Model is ToDoListViewModel toDoListViewModel)
            {
                toDoListViewModel.ToDoSelected?.Execute((sender as ListView).SelectedItem);
            }
        }
    }
}