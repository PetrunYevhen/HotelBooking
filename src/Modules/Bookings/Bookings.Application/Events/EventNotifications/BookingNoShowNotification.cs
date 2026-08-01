using Application.Events;
using Bookings.Domain.Entities.Events;
using Newtonsoft.Json;

namespace Bookings.Application.Events.EventNotifications;

public class BookingNoShowNotification : DomainNotificationBase<BookingNoShowDomainEvent>
{
    [JsonConstructor]
    public BookingNoShowNotification(BookingNoShowDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}
