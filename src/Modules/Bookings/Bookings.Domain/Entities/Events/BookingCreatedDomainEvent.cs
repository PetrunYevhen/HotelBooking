using BuildingBlock.Domain.Events;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.Entities.Events;

public class BookingCreatedDomainEvent : DomainEventBase
{
    public BookingId BookingId { get; }
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    public DateRange BookingDates { get; set; }
    public Money TotalPrice { get; set; }

    public BookingCreatedDomainEvent(
        BookingId bookingId, 
        Guid hotelId,
        Guid roomId, 
        DateRange bookingDates,
        Money totalPrice
        )      
    {
        BookingId = bookingId;
        HotelId = hotelId;
        RoomId = roomId;
        BookingDates = bookingDates;
        TotalPrice = totalPrice;
    }
}