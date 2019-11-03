using System;
using System.Windows.Input;
using ToDo.DataAccess.DataModels.Entities;
using ToDoUICore.MVVMHelper;

namespace ToDo.UI.ViewModel
{
    public class ToDoDetailViewModel : BaseViewModel
    {
        private string title;
        private DateTime creationTime;
        private string description;
        private string color;
        private DateTime deathline;
        private NoteStatus status;

        public int? Id { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }

        public DateTime Deathline
        {
            get => deathline;
            set => SetProperty(ref deathline, value);
        }

        public DateTime CreationTime
        {
            get => creationTime;
            set => SetProperty(ref creationTime, value);
        }

        public NoteStatus Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        private bool isEditMode;

        public bool IsEditMode
        {
            get => isEditMode;
            set => SetProperty(ref isEditMode, value);
        }

        public ICommand SaveChanges { get; set; }
        public ICommand Delete { get; set; }
        public ICommand SetNoteStatusToCompleted { get; set; }
        public ICommand SetNoteStatusToArchived { get; set; }
        public ICommand Edit { get; set; }
        public NoteType ToDoType { get; set; }
        public int TopicId { get; set; }
    }

    public static class ToDoModelDetailExtention
    {
        public static void SetPropertiesFrom(this ToDoDetailViewModel todoViewModel, TodoEntity todoEntity)
        {
            todoViewModel.Color = todoEntity.Color;
            todoViewModel.CreationTime = todoEntity.CreatedAt;
            todoViewModel.Deathline = todoEntity.Deathline;
            todoViewModel.Description = todoEntity.Description;
            todoViewModel.Id = todoEntity.Id;
            todoViewModel.Title = todoEntity.Title;
            todoViewModel.Status = todoEntity.Status;
            todoViewModel.ToDoType = todoEntity.ToDoType;
            todoViewModel.IsEditMode = false;
        }
    }
}