using Application.Events;
using Newtonsoft.Json;
using Reviews.Domain.Entities.Reviews.Events;

namespace Reviews.Application.Events.EventNotifications;

public class ReviewPublishedNotification : DomainNotificationBase<ReviewPublishedDomainEvent>
{
    [JsonConstructor]
    public ReviewPublishedNotification(ReviewPublishedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}