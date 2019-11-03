using EntityFrameworkCoreHelper;
using System;

namespace ToDo.DataAccess.DataModels.Entities
{
    public class TodoEntity : BaseEntity
    {
        public int TopicId { get; set; }
        public TopicEntity Topic { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public NoteStatus Status { get; set; }
        public NoteType ToDoType { get; set; }
        public DateTime Deathline { get; set; }
    }

    public static class ToDoTaskExtention
    {
        public static bool IsMatchFilter(this TodoEntity task,
                                        string searchQuery,
                                        bool filterComleted,
                                        bool filterNotCompleted,
                                        bool filterToday,
                                        bool filterExpired)
        {
            if (task == null)
                return false;

            bool matchedSearchQuery = string.IsNullOrWhiteSpace(searchQuery) ?
                                        true
                                        :
                                        task.Title.ToLower().Contains(searchQuery.ToLower());

            bool marchedFilterComleted = filterComleted ? task.Status == NoteStatus.Completed : true;
            bool marchedFilterExpired = filterExpired ? task.Deathline <= DateTime.Now : true;
            bool marchedFilterNotCompleted = filterNotCompleted ? task.Status == NoteStatus.Active : true;
            bool marchedFilterToday = filterToday ? task.Deathline.Date == DateTime.Today : true;

            return matchedSearchQuery && marchedFilterComleted && marchedFilterNotCompleted && marchedFilterToday && marchedFilterExpired;
        }
    }

    public class NoteTask
    {
    }

    public enum NoteStatus
    {
        Active,
        Completed,
        Archived,
    }

    public enum NoteType
    {
        Remainder,
        Note,
    }

    public enum UserType
    {
        TempUser,
    }

    public class Settings
    {
        public bool AutoLogin { get; set; }
        public bool IsNightMode { get; set; }
    }
}