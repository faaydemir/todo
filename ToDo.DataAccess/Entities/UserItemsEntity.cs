using EntityFrameworkCoreHelper;
using System.Collections.Generic;

namespace ToDo.DataAccess.DataModels.Entities
{
    public class UserBagEntity : BaseEntity
    {
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public IList<TopicEntity> Topics { get; set; }
    }
}