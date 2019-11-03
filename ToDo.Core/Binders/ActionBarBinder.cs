using Prism.Events;
using System.Threading.Tasks;
using ToDo.Core.Messaging;
using ToDo.Core.Services;
using ToDo.Core.ViewModel;
using ToDoUICore.MVVMHelper;

namespace ToDo.Core.Binders
{
    /// <summary>
    /// create view model instance and connect it with services
    /// </summary>
    public class ActionBarBinder : GenericModelBinder<ActionBarViewModel>
    {
        #region props

        private IToDoAppNavigation Navigation { get; }
        private IAppStateService AppStateService { get; }
        public IAuthenticationService AuthenticationService { get; }
        private IAppLogger Logger { get; }
        private IEventAggregator EventAggregator { get; }

        #endregion props

        #region ctor

        public ActionBarBinder(IToDoAppNavigation navigation,
                               IAppStateService appStateService,
                               IAuthenticationService authenticationService,
                               IEventAggregator eventAggregator,
                               IAppLogger logger)
        {
            Navigation = navigation;
            AppStateService = appStateService;
            AuthenticationService = authenticationService;
            Logger = logger;
            EventAggregator = eventAggregator;
        }

        #endregion ctor

        #region methods

        public override async Task<bool> InitlizeAsync()
        {
            TypedModel = new ActionBarViewModel();

            await Logger.LogBindingStarted(this.GetType().Name);

            TypedModel.OpenSettingsPage = new RelayCommand(OpenSettingsPageAsync);
            TypedModel.Login = new RelayCommand(OpenLoginPageAsync);
            TypedModel.Logout = new RelayCommand(LogoutAsync);

            EventAggregator.GetEvent<AuthenticationChangedEvent>().Subscribe(
                async (args) =>
                {
                    await InitView();
                });

            await InitView();
            return true;
        }

        #endregion methods

        #region private_methods

        /// <summary>
        /// Log out
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<bool> LogoutAsync(object obj)
        {
            var isLogout = await AuthenticationService.LogoutAsync();

            if (isLogout)
            {
                // set current user null on app state service
                AppStateService.SetCurrentUser(null);

                //re init user info to view
                await InitView();

                //open auth page
                await Navigation.OpenAuthPage();
            }

            return isLogout;
        }

        private async Task<bool> OpenLoginPageAsync(object obj)
        {
            return await Navigation.OpenAuthPage();
        }

        /// <summary>
        /// get current user and init view
        /// </summary>
        /// <returns></returns>
        private async Task InitView()
        {
            var currentUser = await AppStateService.GetCurrentUser();

            if (currentUser is null)
            {
                TypedModel.IsAuthenticated = false;
                TypedModel.UserName = "";
            }
            else
            {
                TypedModel.IsAuthenticated = true;
                TypedModel.UserName = currentUser.Name;
            }
        }

        /// <summary>
        /// open settings page
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<bool> OpenSettingsPageAsync(object obj)
        {
            //return await Navigation.OpenSettingsPage();
            return await Task.FromResult(false);
        }

        #endregion private_methods
    }
}