using Prism.Events;
using System;
using System.Threading.Tasks;
using ToDo.Core.Binders;
using ToDo.Core.Messaging;
using ToDo.Core.Services;
using ToDo.DataAccess;
using ToDo.DataAccess.DataModels.Entities;
using ToDo.UI.ViewModel;
using ToDoUICore.MVVMHelper;

namespace ToDo.UI.Binder
{
    /// <summary>
    /// Create ToDoDetailViewModel instance and connect it with services
    /// </summary>
    public class ToDoDetailBinder : GenericModelBinder<ToDoDetailViewModel>
    {
        private IToDoAppNavigation NavigationService { get; }
        private IAppStateService AppStateService { get; }
        private IToDoRepository ToDoRepository { get; }
        private IEventAggregator EventAggregator { get; }
        private IAppLogger Logger { get; }

        public ToDoDetailBinder(IToDoRepository todoRepository,
                                IAppStateService appStateService,
                                IToDoAppNavigation navigationService,
                                IEventAggregator eventAggregator,
                                IAppLogger logger)
        {
            EventAggregator = eventAggregator;
            NavigationService = navigationService;
            AppStateService = appStateService;
            ToDoRepository = todoRepository;
            Logger = logger;
        }

        public override Task<bool> InitlizeAsync()
        {
            Logger.LogBindingStarted(this.GetType().Name);
            NavigationService.ToDoChangedEvent += OnToDoChanged;
            TypedModel = new ToDoDetailViewModel()
            {
                SetNoteStatusToArchived = new RelayCommand(async (obj) => await SetNoteStatus(NoteStatus.Archived)),
                SetNoteStatusToCompleted = new RelayCommand(async (obj) => await SetNoteStatus(NoteStatus.Completed)),
                Delete = new RelayCommand(DeleteTodoAsync),
                SaveChanges = new RelayCommand(SaveTodoAsync),
                Edit = new RelayCommand((args) =>
                {
                    TypedModel.IsEditMode = !TypedModel.IsEditMode;
                }),
                IsEditMode = false,
            };

            return Task.FromResult(true);
        }

        private async void OnToDoChanged(object sender, ToDoChangedEventArgs eventArgs)
        {
            if (eventArgs.TodoId.HasValue)
            {
                var selectedTodo = await ToDoRepository.GetAsync(eventArgs.TodoId.Value);
                if (selectedTodo == null)
                {
                    await Logger.LogError($"Can't find todo with id '{eventArgs.TodoId.Value}'");
                }
                //mapper for
                TypedModel.SetPropertiesFrom(selectedTodo);
            }
            else if (eventArgs.TopicId.HasValue)
            {
                TypedModel.IsEditMode = true;
                TypedModel.TopicId = eventArgs.TopicId.Value;
                TypedModel.Id = null;
                TypedModel.Title = "";
                TypedModel.Description = "";
                TypedModel.Deathline = DateTime.Now;
            }
        }

        private async Task<bool> SaveTodoAsync(object obj)
        {
            TypedModel.IsEditMode = false;
            //log
            await Logger.LogInfo($"Updating todo: {TypedModel.Id}");

            bool isUpdated;
            if (!TypedModel.Id.HasValue)
            {
                TodoEntity toDoTask = new TodoEntity
                {
                    Status = TypedModel.Status,
                    Title = TypedModel.Title,
                    Deathline = TypedModel.Deathline,
                    Color = TypedModel.Color,
                    Description = TypedModel.Description,
                    TopicId = TypedModel.TopicId
                };

                var insertedItem = await ToDoRepository.AddAsync(toDoTask);
                await ToDoRepository.SaveChangesAsync();
                if (insertedItem != null)
                {
                    TypedModel.Id = insertedItem.Id;
                    isUpdated = true;
                }
                else
                {
                    isUpdated = false;
                }
            }
            else
            {
                TodoEntity toDoTask = await ToDoRepository.GetAsync(TypedModel.Id.Value);
                toDoTask.Status = TypedModel.Status;
                toDoTask.Title = TypedModel.Title;
                toDoTask.Deathline = TypedModel.Deathline;
                toDoTask.Color = TypedModel.Color;
                toDoTask.Description = TypedModel.Description;
                isUpdated = await ToDoRepository.UpdateAsync(toDoTask);
                await ToDoRepository.SaveChangesAsync();
                if (isUpdated)
                {
                    EventAggregator.GetEvent<TodoUpdatedEvent>().Publish(toDoTask.Id);
                }
                //update
            }

            return isUpdated;
        }

        private async Task<bool> DeleteTodoAsync(object obj)
        {
            //log
            await Logger.LogInfo($"Deleting Todo: {TypedModel.Id}");

            //delete todo
            bool isDeleted = await ToDoRepository.DeleteAsync(TypedModel.Id.Value);
            await ToDoRepository.SaveChangesAsync();
            //notify related views
            if (isDeleted)
            {
                EventAggregator.GetEvent<TodoDeletedEvent>().Publish(TypedModel.Id.Value);
            }
            return isDeleted;
        }

        private async Task SetNoteStatus(NoteStatus status)
        {
            bool isSet = await ToDoRepository.SetTaskStatusAsync(TypedModel.Id.Value, status);

            if (isSet)
            {
                TypedModel.Status = status;
                EventAggregator.GetEvent<TodoUpdatedEvent>().Publish(TypedModel.Id.Value);
            }
        }
    }
}