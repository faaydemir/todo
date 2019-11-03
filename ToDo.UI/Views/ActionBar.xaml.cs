using ToDo.Core.Binders;

namespace ToDo.UI.Views
{
    /// <summary>
    /// Interaction logic for ActionBar.xaml
    /// </summary>
    public partial class ActionBar : UserControlBase
    {
        public ActionBar()
        {
            BindAsync<ActionBarBinder>();
            InitializeComponent();
        }
    }
}