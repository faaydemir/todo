using EntityFrameworkCoreHelper;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.DataAccess.Repositories
{
    public interface IUserRepository : IRepository<UserEntity, int>
    {
    }
}