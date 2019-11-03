using System.Threading.Tasks;
using ToDo.Core.Services;
using ToDo.Core.ViewModel;
using ToDoUICore.MVVMHelper;

namespace ToDo.Core.Binders
{
    /// <summary>
    /// Create ToDoDetailViewModel instance and connect it with services
    /// </summary>
    public class LoginModelBinder : GenericModelBinder<AuthenticationViewModel>
    {
        private IAuthenticationService AuthenticationService { get; }
        private IToDoAppNavigation TodoAppNavigation { get; }
        private IAppStateService AppStateService { get; }
        private IAppLogger AppLogger { get; }

        public LoginModelBinder(IAuthenticationService authenticationService,
                                IToDoAppNavigation todoAppNavigation,
                                IAppStateService appStateService,
                                IAppLogger appLogger)
        {
            AuthenticationService = authenticationService;
            TodoAppNavigation = todoAppNavigation;
            AppStateService = appStateService;
            AppLogger = appLogger;
        }

        public override Task<bool> InitlizeAsync()
        {
            TypedModel = new AuthenticationViewModel();

            TypedModel.Login = new RelayCommand(LoginAsync);
            TypedModel.Singup = new RelayCommand(SingupAsync);

            return Task.FromResult(true);
        }

        private async Task<bool> SingupAsync(object obj)
        {
            var result = await AuthenticationService.SingupAsync(TypedModel.UserName,
                                                            TypedModel.Mail,
                                                            TypedModel.Password);

            if (result.IsAuthSucceed)
            {
                AppStateService.SetCurrentUser(result.User);
                await TodoAppNavigation.OpenTodoPage();
            }
            else
            {
                TypedModel.SingupError = result.Message;
            }

            return result.IsAuthSucceed;
        }

        private async Task<bool> LoginAsync(object obj)
        {
            var result = await AuthenticationService.LoginAsync(TypedModel.UserName, TypedModel.Password);

            if (result.IsAuthSucceed)
            {
                AppStateService.SetCurrentUser(result.User);
                await TodoAppNavigation.OpenTodoPage();
            }
            else
            {
                TypedModel.LoginError = result.Message;
            }
            return result.IsAuthSucceed;
        }
    }
}