using Application.Events;
using Bookings.Domain.Entities.Events;
using Newtonsoft.Json;

namespace Bookings.Application.Events.EventNotifications;

public class BookingCreatedNotification : DomainNotificationBase<BookingCreatedDomainEvent>
{
    [JsonConstructor]
    public BookingCreatedNotification(BookingCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}