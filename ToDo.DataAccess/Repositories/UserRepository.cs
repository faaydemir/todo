using EntityFrameworkCoreHelper;
using Microsoft.EntityFrameworkCore;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        public UserRepository(
            DbContext context) : base(context)
        {
        }
    }
}