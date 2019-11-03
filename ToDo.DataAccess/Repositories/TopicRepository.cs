using EntityFrameworkCoreHelper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.DataAccess.Repositories
{
    public class TopicRepository : GenericRepository<TopicEntity>, ITopicRepository
    {
        public TopicRepository(DbContext context = null) : base(context)
        {
        }

        public Task<List<TopicEntity>> GetAllUserTopicsAsync(int userBagId)
        {
            var userTopics = GetAll().Where(x => x.UserBagId == userBagId).Include(x => x.TodoList).ToList();
            return Task.FromResult(userTopics);
        }
    }
}