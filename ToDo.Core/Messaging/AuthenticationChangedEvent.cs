using Prism.Events;
using ToDo.Core.Model;

namespace ToDo.Core.Messaging
{
    public class AuthenticationChangedEvent : PubSubEvent<User>
    {
    }
}