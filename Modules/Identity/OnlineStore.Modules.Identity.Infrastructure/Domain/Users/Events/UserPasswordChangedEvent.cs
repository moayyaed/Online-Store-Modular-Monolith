using Common.Domain.Types;

namespace OnlineStore.Modules.Identity.Infrastructure.Domain.Users.Events
{
    public class UserPasswordChangedEvent : DomainEventBase
    {
        public UserPasswordChangedEvent(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}
