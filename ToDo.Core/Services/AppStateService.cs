using Prism.Events;
using System.Threading.Tasks;
using ToDo.Core.Messaging;
using ToDo.Core.Model;

namespace ToDo.Core.Services
{
    /// <summary>
    /// Service to keep app current state
    /// </summary>
    public class AppStateService : IAppStateService
    {
        #region Props

        private IAppLogger AppLogger { get; }

        #endregion Props

        #region fields

        private User CurrentUser;
        private IEventAggregator EventAggregator;

        #endregion fields

        #region ctor

        public AppStateService(IEventAggregator eventAggregator,
                               IAppLogger appLogger)
        {
            EventAggregator = eventAggregator;
            AppLogger = appLogger;
        }

        #endregion ctor

        #region methods

        public Task<User> GetCurrentUser()
        {
            return Task.FromResult(CurrentUser);
        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
            EventAggregator.GetEvent<AuthenticationChangedEvent>().Publish(CurrentUser);
        }

        public void RemoveCurrentUser()
        {
            SetCurrentUser(null);
        }

        #endregion methods
    }
}