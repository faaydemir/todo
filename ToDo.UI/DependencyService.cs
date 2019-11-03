using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;
using System.Threading.Tasks;
using ToDo.Core.Binders;
using ToDo.Core.Services;
using ToDo.DataAccess;
using ToDo.DataAccess.Repositories;
using ToDo.UI.Binder;
using ToDo.UI.Views;
using Utils;

namespace ToDoUICore.AppService
{
    public class DependencyServiceSingleton : SingletonBase<DependencyService>
    {
    }

    public class DependencyService : IDependencyService
    {
        private static ServiceProvider serviceProvider;

        public DependencyService()
        {
            Initlize();
        }

        private void Initlize()
        {
            var collection = new ServiceCollection();

            //Add Services
            collection.AddSingleton(typeof(IDependencyService), typeof(DependencyService));
            collection.AddSingleton(typeof(IAppLogger), typeof(AppLogger));
            collection.AddSingleton(typeof(ILogger), typeof(AppLogger));
            collection.AddSingleton(typeof(IToDoAppNavigation), typeof(ToDoAppNavigation));
            collection.AddSingleton(typeof(IAppStateService), typeof(AppStateService));
            collection.AddSingleton(typeof(IAuthenticationService), typeof(AuthenticationService));

            collection.AddScoped(typeof(DbContext), typeof(TodoDbContext));
            collection.AddScoped(typeof(TodoDbContext), typeof(TodoDbContext));
            collection.AddScoped(typeof(IToDoRepository), typeof(TodoRepository));
            collection.AddScoped(typeof(ITopicRepository), typeof(TopicRepository));
            collection.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            collection.AddScoped(typeof(IEventAggregator), typeof(EventAggregator));
            collection.AddScoped(typeof(IEncryptionService), typeof(EncryptionService));

            //Add Binder
            collection.AddSingleton(typeof(ToDoListBinder), typeof(ToDoListBinder));
            collection.AddSingleton(typeof(ToDoDetailBinder), typeof(ToDoDetailBinder));
            collection.AddSingleton(typeof(ActionBarBinder), typeof(ActionBarBinder));
            collection.AddSingleton(typeof(LoginModelBinder), typeof(LoginModelBinder));

            //Add View Model

            serviceProvider = collection.BuildServiceProvider();
        }

        public Task<object> GetAsync(Type type)
        {
            return Task.FromResult(serviceProvider.GetService(type));
        }

        public Task<T> GetAsync<T>()
        {
            return Task.FromResult(serviceProvider.GetService<T>());
        }
    }
}