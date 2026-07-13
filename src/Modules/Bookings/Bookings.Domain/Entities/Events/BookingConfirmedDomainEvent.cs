using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.Entities.Events;

public class BookingConfirmedDomainEvent : DomainEventBase
{
    public Guid BookingId { get; }
    public Guid RoomId { get; } 
    public Guid UserId { get; }
    public string RecipientEmail { get; }
    public DateRange BookingDates { get; }

    public BookingConfirmedDomainEvent(Guid bookingId, Guid roomId, DateRange bookingDates, Guid userId, string recipientEmail)
    {
        BookingId = bookingId;
        RoomId = roomId;
        BookingDates = bookingDates;
        UserId = userId;
        RecipientEmail = recipientEmail;
    }
}