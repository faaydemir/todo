using EFCoreUtils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.DataAccess.DataModels.Entities;
using ToDo.DataAccess.Repositories;

namespace ToDo.Core.Mock
{
    public class MockTopicRepository : MockRepository<TopicEntity>, ITopicRepository
    {
        public MockTopicRepository()
        {
            Entities = MockEntityGenerator.CreateTopic(3);
        }

        public async Task<List<TopicEntity>> GetAllUserTopicsAsync(int userId)
        {
            return await Task.FromResult(GetAll().ToList());
        }
    }
}