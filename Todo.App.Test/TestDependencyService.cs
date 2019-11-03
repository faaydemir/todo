using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.Threading.Tasks;
using ToDo.Core.Mock;
using ToDo.Core.Services;
using ToDo.DataAccess;
using ToDo.DataAccess.Repositories;
using ToDo.UI.Binder;

namespace Todo.App.Test
{
    public class TestDependencyService : IDependencyService
    {
        private static ServiceProvider serviceProvider;

        public TestDependencyService()
        {
            Initlize();
        }

        private void Initlize()
        {
            var collection = new ServiceCollection();

            collection.AddSingleton(typeof(IToDoRepository), typeof(MockToDoRepository));
            collection.AddSingleton(typeof(ITopicRepository), typeof(MockTopicRepository));
            collection.AddSingleton(typeof(IEventAggregator), typeof(EventAggregator));
            collection.AddSingleton(typeof(IToDoAppNavigation), typeof(MockToDoAppNavigation));

            collection.AddScoped(typeof(IAppLogger), typeof(AppLogger));
            collection.AddScoped(typeof(IAppStateService), typeof(MockAppStateService));
            collection.AddScoped(typeof(ToDoDetailBinder), typeof(ToDoDetailBinder));

            //Add Binder

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