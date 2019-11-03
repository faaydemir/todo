using ToDo.UI.Binder;

namespace ToDo.UI.Views
{
    /// <summary>
    /// Interaction logic for NoteDetail.xaml
    /// </summary>
    public partial class ToDoDetail : UserControlBase
    {
        public ToDoDetail()
        {
            InitializeComponent();
            BindAsync<ToDoDetailBinder>();
        }
    }
}