using Prism.Events;

namespace ToDo.Core.Messaging
{
    public class TodoDeletedEvent : PubSubEvent<int>
    {
    }
}