using System.Windows.Input;
using ToDoUICore.MVVMHelper;

namespace ToDo.Core.ViewModel
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private string userName = "testuser";
        private string rePassword = "test";
        private string password = "test";
        private string mail;
        private string loginError;
        private string singupError;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Mail
        {
            get => mail;
            set => SetProperty(ref mail, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string RePassword
        {
            get => rePassword;
            set => SetProperty(ref rePassword, value);
        }

        public string LoginError
        {
            get => loginError;
            set => SetProperty(ref loginError, value);
        }

        public string SingupError
        {
            get => singupError;
            set => SetProperty(ref singupError, value);
        }

        public ICommand Login { get; set; }

        public ICommand Singup { get; set; }
    }
}