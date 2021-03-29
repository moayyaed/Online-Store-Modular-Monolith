using System.Threading.Tasks;

namespace Infrastructure.Messaging.Events
{
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}