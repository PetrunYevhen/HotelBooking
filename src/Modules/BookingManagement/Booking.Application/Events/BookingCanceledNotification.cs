using Application.Events;
using BookingManagement.Domain.Entities.Events;
using MediatR;
using Newtonsoft.Json;

namespace BookingManagement.Application.Events;

public class BookingCanceledNotification : DomainNotificationBase<BookingCanceledDomainEvent>
{
    [JsonConstructor]
    public BookingCanceledNotification(BookingCanceledDomainEvent domainEvent, Guid id) 
        : base(domainEvent, id)
    {
    }
}