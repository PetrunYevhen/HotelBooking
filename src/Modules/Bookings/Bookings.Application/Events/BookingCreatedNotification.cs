using Application.Events;
using Bookings.Domain.Entities.Events;
using Newtonsoft.Json;

namespace Bookings.Application.Events;

public class BookingCreatedNotification : DomainNotificationBase<BookingCreatedDomainEvent>
{
    [JsonConstructor]
    public BookingCreatedNotification(BookingCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}