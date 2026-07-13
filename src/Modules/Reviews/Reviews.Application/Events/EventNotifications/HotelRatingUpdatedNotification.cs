using Application.Events;
using Newtonsoft.Json;
using Reviews.Domain.Entities.Snapshot.Events;

namespace Reviews.Application.Events.EventNotifications;

public class HotelRatingUpdatedNotification : DomainNotificationBase<HotelRatingUpdatedDomainEvent>
{
    [JsonConstructor]
    public HotelRatingUpdatedNotification(HotelRatingUpdatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}