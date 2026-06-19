using Application.Events;
using Bookings.Domain.Entities.Events;
using Newtonsoft.Json;

namespace Bookings.Application.Events.EventNotifications;

public class BookingConfirmedNotification : DomainNotificationBase<BookingConfirmedDomainEvent>
{
    
    [JsonConstructor]
    public BookingConfirmedNotification(BookingConfirmedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}
