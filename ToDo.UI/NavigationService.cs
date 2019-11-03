#region usings

using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Windows;
using ToDo.Core.Services;
using ToDo.UI;
using ToDo.UI.Pages;
using TODO;
using WpfUtils.Navigation;

#endregion usings

namespace ToDoUICore.AppService
{
    /// <summary>
    /// NavigationService
    /// </summary>
    public class ToDoAppNavigation : NavigationServiceBase<UserControlBase>, IToDoAppNavigation
    {
        public MainWindow MainWindow { get; }

        public event ToDoChangedEventHandler ToDoChangedEvent;

        public ToDoAppNavigation(ILogger logger) : base(logger)
        {
            MainWindow = Application.Current.MainWindow as MainWindow;
            AddPage(typeof(TodoPage), "TodoPage", false);
            AddPage(typeof(AuthenticationPage), "CreateAccountPage", false);
        }

        public async Task<bool> OpenAuthPage()
        {
            return await SetCurrentPageAsync(typeof(AuthenticationPage));
        }

        public async Task<bool> OpenTodoPage()
        {
            return await SetCurrentPageAsync(typeof(TodoPage));
        }

        //TODO move to EventAggregator
        public Task<bool> OpenToDoDetailAsync(int todoId)
        {
            ToDoChangedEvent?.Invoke(this, new ToDoChangedEventArgs { TodoId = todoId });
            return Task.FromResult(true);
        }

        //TODO move to EventAggregator
        public Task<bool> OpenToDoDetailForNew(int topicId)
        {
            ToDoChangedEvent?.Invoke(this, new ToDoChangedEventArgs { TopicId = topicId });
            return Task.FromResult(true);
        }

        public override Task<bool> OpenPageAsync(UserControlBase page)
        {
            Logger.LogInformation($"Page Opened: { page.GetType().Name}");
            MainWindow.PageContainer.Content = page;
            return Task.FromResult(true);
        }
    }
}