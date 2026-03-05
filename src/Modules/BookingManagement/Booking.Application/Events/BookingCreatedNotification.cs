using Application.Events;
using BookingManagement.Domain.Entities.Events;
using Newtonsoft.Json;

namespace BookingManagement.Application.Events;

public class BookingCreatedNotification : DomainNotificationBase<BookingCreatedDomainEvent>
{
    [JsonConstructor]
    public BookingCreatedNotification(BookingCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}