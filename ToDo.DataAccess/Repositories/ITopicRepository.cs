using EntityFrameworkCoreHelper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.DataAccess.Repositories
{
    public interface ITopicRepository : IRepository<TopicEntity, int>
    {
        Task<List<TopicEntity>> GetAllUserTopicsAsync(int userBagId);
    }
}