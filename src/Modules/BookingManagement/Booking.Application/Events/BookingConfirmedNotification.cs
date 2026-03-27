using Application.Events;
using BookingManagement.Domain.Entities.Events;
using MediatR;
using Newtonsoft.Json;

namespace BookingManagement.Application.Events;

public class BookingConfirmedNotification : DomainNotificationBase<BookingConfirmedDomainEvent>
{
    [JsonConstructor]
    public BookingConfirmedNotification(BookingConfirmedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
    {
    }
}