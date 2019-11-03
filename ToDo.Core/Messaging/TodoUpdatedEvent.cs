using Prism.Events;

namespace ToDo.Core.Messaging
{
    public class TodoUpdatedEvent : PubSubEvent<int>
    {
    }
}