using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.Core.Model
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public int UserBagId { get; set; }

        public static User From(UserEntity user)
        {
            if (user == null)
            {
                return null;
            }

            return new User()
            {
                Id = user.Id,
                Name = user.Name,
                Mail = user.EmailAddress,
                UserBagId = user.UserBagId,
            };
        }
    }
}