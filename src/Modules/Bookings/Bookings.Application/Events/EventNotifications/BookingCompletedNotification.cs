using Application.Events;
using Bookings.Domain.Entities.Events;
using Newtonsoft.Json;

namespace Bookings.Application.Events.EventNotifications;

public class BookingCompletedNotification : DomainNotificationBase<BookingCompletedDomainEvent>
{
    [JsonConstructor]
    public BookingCompletedNotification(BookingCompletedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}
