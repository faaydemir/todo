using EntityFrameworkCoreHelper;
using System.Collections.Generic;

namespace ToDo.DataAccess.DataModels.Entities
{
    public enum ItemStatus
    {
        Active = 0,
        Archived = 1,
    }

    public class TopicEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public ItemStatus Status { get; set; }
        public IList<TodoEntity> TodoList { get; set; }
        public int UserBagId { get; set; }
        public UserBagEntity UserBag { get; set; }
    }
}