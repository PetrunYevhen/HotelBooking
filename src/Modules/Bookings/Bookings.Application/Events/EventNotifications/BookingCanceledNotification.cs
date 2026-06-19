using Application.Events;
using Bookings.Domain.Entities.Events;
using Newtonsoft.Json;

namespace Bookings.Application.Events.EventNotifications;

public class BookingCanceledNotification : DomainNotificationBase<BookingCanceledDomainEvent>
{
    [JsonConstructor]
    public BookingCanceledNotification(BookingCanceledDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}
