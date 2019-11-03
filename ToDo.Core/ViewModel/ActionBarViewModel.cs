using System.Windows.Input;
using ToDoUICore.MVVMHelper;

namespace ToDo.Core.ViewModel
{
    public class ActionBarViewModel : BaseViewModel
    {
        private string userName;
        private bool isAuthenticated;

        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set => SetProperty(ref isAuthenticated, value);
        }

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public ICommand OpenSettingsPage { get; set; }
        public ICommand Login { get; set; }
        public ICommand Logout { get; set; }
    }
}