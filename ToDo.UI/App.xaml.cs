using System.Windows;
using ToDoUICore;

namespace TODO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            var appMainService = new AppMainService();
            await appMainService.InitilizeAsync(this);
            base.OnStartup(e);
        }
    }
}