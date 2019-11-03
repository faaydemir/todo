using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Core.Binders;
using ToDo.Core.Messaging;
using ToDo.Core.Model;
using ToDo.Core.Services;
using ToDo.Core.ViewModel;
using ToDo.DataAccess;
using ToDo.DataAccess.DataModels.Entities;
using ToDo.DataAccess.Repositories;
using ToDoUICore.MVVMHelper;

namespace ToDo.UI.Views
{
    /// <summary>
    /// create ToDoListViewModel instance and connect it with services
    /// </summary>
    public class ToDoListBinder : GenericModelBinder<ToDoListViewModel>
    {
        #region fields

        private IToDoAppNavigation NavigationService;
        private IAppStateService AppStateService;
        private ITopicRepository TopicRepository;
        private IToDoRepository ToDoRepository;
        private IAppLogger Logger;
        private IEventAggregator EventAggregator;

        #endregion fields

        #region constructors

        public ToDoListBinder(IToDoAppNavigation navigationService,
                              ITopicRepository topicRepository,
                              IAppStateService appStateService,
                              IToDoRepository toDoRepository,
                              IEventAggregator eventAggregator,
                              IAppLogger logger)
        {
            EventAggregator = eventAggregator;
            NavigationService = navigationService;
            ToDoRepository = toDoRepository;
            TopicRepository = topicRepository;
            AppStateService = appStateService;
            Logger = logger;
        }

        #endregion constructors

        #region methods

        /// <summary>
        /// Bind Services to ViewModel
        /// </summary>
        /// <returns>if service initlized</returns>
        public override async Task<bool> InitlizeAsync()
        {
            await Logger.LogBindingStarted(this.GetType().Name);

            TypedModel = new ToDoListViewModel
            {
                ToDoSelected = new RelayCommand(OpenToDoPageDetail),
                ToDoArchived = new RelayCommand(ToDoStatusChanged),
                ApplyFilter = new RelayCommand(ApplyFilter),
                AddTodo = new RelayCommand(AddToDoAsync),
                CloseFilterWindowsOpened = new RelayCommand((args) =>
                {
                    TypedModel.IsFilterWindowsOpened = false;
                }),

                ChangeFilterWindowsOpened = new RelayCommand((args) =>
                {
                    if (args is bool isFilterWindowsOpened)
                    {
                        TypedModel.IsFilterWindowsOpened = isFilterWindowsOpened;
                    }
                    else
                    {
                        TypedModel.IsFilterWindowsOpened = !TypedModel.IsFilterWindowsOpened;
                    }
                }),

                CancelTopic = new RelayCommand((arg) =>
                {
                    TypedModel.NewTopicName = "";
                    TypedModel.IsAddTopicFromVisible = false;
                }),

                AddNewTopic = new RelayCommand(async (arg) =>
                {
                    if (!TypedModel.IsAddTopicFromVisible)
                    {
                        TypedModel.IsAddTopicFromVisible = true;
                    }
                    else
                    {
                        await AddTopicAsync();
                        TypedModel.IsAddTopicFromVisible = false;
                    }
                }),
            };
            EventAggregator.GetEvent<AuthenticationChangedEvent>().Subscribe(
                async (obj) => await InitTopicsAsync()
                );

            EventAggregator.GetEvent<TodoUpdatedEvent>().Subscribe(
                 async (obj) => await InitTopicsAsync()
                 );

            await InitTopicsAsync();

            return true;
        }

        /// <summary>
        /// Add topic with entered name
        /// </summary>
        /// <returns></returns>
        private async Task AddTopicAsync()
        {
            if (!string.IsNullOrWhiteSpace(TypedModel.NewTopicName))
            {
                // check if user authenticated
                var currentUser = await AppStateService.GetCurrentUser();
                if (currentUser != null)
                {
                    try
                    {
                        //add item
                        var addedItem = await TopicRepository.AddAsync(new TopicEntity()
                        {
                            Name = TypedModel.NewTopicName,
                            UserBagId = currentUser.UserBagId,
                        });

                        await TopicRepository.SaveChangesAsync();

                        //re init topic list
                        if (addedItem != null)
                        {
                            await InitTopicsAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await Logger.LogError(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// init todo list and topics
        /// </summary>
        /// <returns></returns>
        private async Task InitTopicsAsync()
        {
            User user = await AppStateService.GetCurrentUser();
            if (user == null)
            {
                TypedModel.Topics = new ObservableCollection<TopicViewModel>();
            }
            else
            {
                var topics = await TopicRepository.GetAllUserTopicsAsync(user.Id);
                TypedModel.Topics = topics == null ?
                   new ObservableCollection<TopicViewModel>() :
                   new ObservableCollection<TopicViewModel>(topics.Select(x => TopicViewModel.From(x)));

                await ApplyFilter();
            }
        }

        /// <summary>
        /// Add todo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<bool> AddToDoAsync(object obj)
        {
            var topic = obj as TopicViewModel;
            return await NavigationService.OpenToDoDetailForNew(topic.Id);
        }

        /// <summary>
        /// filter to do list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<bool> ApplyFilter(object obj = null)
        {
            //close filter window
            TypedModel.IsFilterWindowsOpened = false;
            var topics = await TopicRepository.GetAllAsync();
            //TODO make filtering in task
            // filter list
            var filteredList = topics.Select(topic =>
                            {
                                if (topic.TodoList != null && topic.TodoList.Any())
                                {
                                    topic.TodoList = topic.TodoList.Where(toDo =>
                                           toDo.IsMatchFilter(
                                               searchQuery: TypedModel.SearchQuery,
                                               filterComleted: TypedModel.FilterComleted,
                                               filterToday: TypedModel.FilterToday,
                                               filterNotCompleted: TypedModel.FilterNotCompleted,
                                               filterExpired: TypedModel.FilterExpired))
                                            .ToList();
                                }
                                return topic;
                            })
                            .ToList();

            TypedModel.Topics = filteredList == null ?
                   new ObservableCollection<TopicViewModel>() :
                   new ObservableCollection<TopicViewModel>(filteredList.Select(x => TopicViewModel.From(x)));

            // update topics observable collection

            return await Task.FromResult(false);
        }

        /// <summary>
        /// Change ToDo Task Status
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>return if operation success as bool</returns>
        private async Task<bool> ToDoStatusChanged(object obj)
        {
            var changedTask = obj as ToDoViewModel;
            return await ToDoRepository.SetTaskStatusAsync(changedTask.Id, changedTask.Status);
        }

        /// <summary>
        /// open todo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>return if operation success as bool </returns>
        private async Task<bool> OpenToDoPageDetail(object obj)
        {
            var openedTask = obj as ToDoViewModel;
            return await NavigationService.OpenToDoDetailAsync(openedTask.Id);
        }

        #endregion methods
    }
}