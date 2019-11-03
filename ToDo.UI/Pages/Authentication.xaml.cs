using ToDo.Core.Binders;

namespace ToDo.UI.Pages
{
    /// <summary>
    /// Interaction logic for CreateAccountPage.xaml
    /// </summary>
    public partial class AuthenticationPage : UserControlBase
    {
        public AuthenticationPage()
        {
            InitializeComponent();
            BindAsync<LoginModelBinder>();
        }
    }
}