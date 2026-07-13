using Application.Events;
using Newtonsoft.Json;
using Reviews.Domain.Entities.Reviews.Events;

namespace Reviews.Application.Events.EventNotifications;

public class ReviewCreatedNotification : DomainNotificationBase<ReviewCreatedDomainEvent>
{
    [JsonConstructor]
    public ReviewCreatedNotification(ReviewCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }

    
}