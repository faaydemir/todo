using EntityFrameworkCoreHelper;

namespace ToDo.DataAccess.DataModels.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int UserBagId { get; set; }
        public virtual UserBagEntity UserBag { get; set; }

        public UserEntity()
        {
            UserBag = new UserBagEntity();
        }
    }
}