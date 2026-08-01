using Application.Events;
using Bookings.Domain.Entities.Events;
using Newtonsoft.Json;

namespace Bookings.Application.Events.EventNotifications;

public class BookingExpiredNotification : DomainNotificationBase<BookingExpiredDomainEvent>
{
    [JsonConstructor]
    public BookingExpiredNotification(BookingExpiredDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}
