using System;
using System.Collections.Generic;
using ToDo.DataAccess.DataModels.Entities;

namespace ToDo.Core.Mock
{
    public static class MockEntityGenerator
    {
        public static int topicIndex = 0;
        public static int todoIndex = 0;

        public static IList<TopicEntity> CreateTopic(int count)
        {
            var random = new Random();
            var topicList = new List<TopicEntity>();
            for (int i = 0; i < count; i++)
            {
                topicList.Add(new TopicEntity()
                {
                    Id = topicIndex++,
                    Color = "",
                    Name = $"Topic {topicIndex}",
                    Status = (ItemStatus)(i % 2),
                    TodoList = CreateToDo(random.Next() % 10),
                }); ;
            }

            return topicList;
        }

        public static IList<TodoEntity> CreateToDo(int count)
        {
            var random = new Random();
            var todoList = new List<TodoEntity>();
            for (int i = 0; i < count; i++)
            {
                todoList.Add(new TodoEntity()
                {
                    Id = todoIndex++,
                    Color = "",
                    Title = $"Title {todoIndex++}",
                    Status = (NoteStatus)(i % 3),
                    Description = $"Description {todoIndex++}",
                    Deathline = DateTime.Now.AddHours(-24 + random.Next() % 48),
                }); ;
            }

            return todoList;
        }
    }
}