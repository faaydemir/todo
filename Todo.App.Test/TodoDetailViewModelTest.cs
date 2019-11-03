using System;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Core.Services;
using ToDo.DataAccess;
using ToDo.DataAccess.DataModels.Entities;
using ToDo.UI.Binder;
using ToDo.UI.ViewModel;
using Xunit;

namespace Todo.App.Test
{
    public class TodoDetailViewModelTest
    {
        private TestDependencyService DependencyService;

        public TodoDetailViewModelTest()
        {
            DependencyService = new TestDependencyService();
        }

        [Fact]
        public async Task Should_Open_TodoAsync()
        {
            var todoRepository = await DependencyService.GetAsync<IToDoRepository>();
            var todoDetailViewModel = await CreateTodoDetailViewModelAsync();
            var navigationService = await DependencyService.GetAsync<IToDoAppNavigation>();
            var first = todoRepository.GetAll().First();

            await navigationService.OpenToDoDetailAsync(first.Id);

            Assert.Equal(first.Deathline, todoDetailViewModel.Deathline);
            Assert.Equal(first.Title, todoDetailViewModel.Title);
            Assert.Equal(first.Id, todoDetailViewModel.Id);
            Assert.Equal(first.TopicId, todoDetailViewModel.TopicId);
            Assert.Equal(first.Status, todoDetailViewModel.Status);
        }

        [Fact]
        public async Task Should_Add_TodoAsync()
        {
            var todoRepository = await DependencyService.GetAsync<IToDoRepository>();
            var TodoDetailViewModel = await CreateTodoDetailViewModelAsync();

            var title = "TestTitle";
            var description = "TestDescription";
            var topicId = 1;

            TodoDetailViewModel.Id = null;
            TodoDetailViewModel.Deathline = DateTime.Now.AddDays(1);
            TodoDetailViewModel.Title = title;
            TodoDetailViewModel.Description = description;
            TodoDetailViewModel.TopicId = topicId;

            TodoDetailViewModel.SaveChanges.Execute(null);

            var insertedTodo = todoRepository.GetAll().FirstOrDefault(x => x.Title == title &&
                                                                      x.Description == description &&
                                                                      x.TopicId == topicId);
            Assert.NotNull(insertedTodo);
            Assert.Equal(description, insertedTodo.Description);
            Assert.Equal(title, insertedTodo.Title);
            Assert.Equal(topicId, insertedTodo.TopicId);
        }

        [Fact]
        public async Task Should_Update_TodoAsync()
        {
            var todoRepository = await DependencyService.GetAsync<IToDoRepository>();

            var TodoDetailViewModel = await CreateTodoDetailViewModelAsync();
            TodoEntity todoEntity = todoRepository.GetAll().First();

            string title = todoEntity.Title + "v2";
            string description = todoEntity.Description + "v2";
            DateTime deathline = DateTime.Now.AddDays(2);

            //different status than current one
            NoteStatus status = (todoEntity.Status == NoteStatus.Completed) ? NoteStatus.Active : NoteStatus.Completed;

            TodoDetailViewModel.SetPropertiesFrom(todoEntity);
            TodoDetailViewModel.Deathline = deathline;
            TodoDetailViewModel.Title = title;
            TodoDetailViewModel.Description = description;
            TodoDetailViewModel.Status = status;
            TodoDetailViewModel.SaveChanges.Execute(null);

            var updatedTodo = await todoRepository.GetAsync(todoEntity.Id);

            Assert.NotNull(updatedTodo);
            Assert.Equal(deathline, updatedTodo.Deathline);
            Assert.Equal(description, updatedTodo.Description);
            Assert.Equal(title, updatedTodo.Title);
            Assert.Equal(status, updatedTodo.Status);
        }

        [Fact]
        public async Task Should_Delete_TodoAsync()
        {
            var todoRepository = await DependencyService.GetAsync<IToDoRepository>();
            var todoDetailViewModel = await CreateTodoDetailViewModelAsync();

            TodoEntity todoEntity = todoRepository.GetAll().First();

            todoDetailViewModel.SetPropertiesFrom(todoEntity);

            todoDetailViewModel.Delete.Execute(null);

            var deletedTodo = await todoRepository.GetAsync(todoEntity.Id);

            Assert.Null(deletedTodo);
        }

        [Fact]
        public async Task Should_Open_EditModeAsync()
        {
            ToDoDetailViewModel TodoDetailViewModel = await CreateTodoDetailViewModelAsync();
            TodoDetailViewModel.Edit.Execute(null);

            Assert.True(TodoDetailViewModel.IsEditMode);
        }

        private async Task<ToDoDetailViewModel> CreateTodoDetailViewModelAsync()
        {
            var binder = await DependencyService.GetAsync<ToDoDetailBinder>();
            await binder.InitlizeAsync();
            return binder.TypedModel;
        }
    }
}