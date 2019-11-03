using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.DataAccess;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.Core.Mock
{
    public class Seed
    {
        public static async Task<bool> SeedInitialDataAsync(TodoDbContext dbContext, int topicCount = 3, int todoCount = 5)
        {
            if (dbContext.Users.Any())
            {
                return false;
            }
            var userEntity = new UserEntity()
            {
                Name = "testuser",
                EmailAddress = "testuser@test.com",
                Password = "test",
                UserBag = new UserBagEntity(),
            };

            dbContext.Users.Add(userEntity);
            await dbContext.SaveChangesAsync();

            for (int i = 0; i < topicCount; i++)
            {
                var topic = new TopicEntity()
                {
                    Color = "",
                    Name = $"Topic {i}",
                    Status = (ItemStatus)(i % 2),
                    UserBagId = userEntity.UserBagId,
                };
                await dbContext.Topics.AddAsync(topic);
                await dbContext.SaveChangesAsync();
            }

            var topics = dbContext.Topics.ToList();
            foreach (var topic in topics)
            {
                var random = new Random();
                var todoList = new List<TodoEntity>();
                for (int i = 0; i < todoCount; i++)
                {
                    var todo = new TodoEntity()
                    {
                        Color = "",
                        Title = $"Title {i}",
                        Status = (NoteStatus)(i % 3),
                        Description = $"Description {i}",
                        Deathline = DateTime.Now.AddHours(-24 + random.Next() % 48),
                        TopicId = topic.Id,
                    };
                    await dbContext.Todos.AddAsync(todo);
                    await dbContext.SaveChangesAsync();
                }
            }

            return true;
        }
    }
}