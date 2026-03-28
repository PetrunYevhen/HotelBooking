using Bookings.Domain.Entities.Events;
using Bookings.Domain.Enums;
using BuildingBlock.Domain;

namespace Bookings.Domain.Entities;

public class Booking : Entity
{
    public BookingId BookingId { get; set; }
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public BookingStatus Status { get; set; }

    private Booking() {} // EF Domain requires a parameterless constructor for entity instantiation
    
    public Booking(
        BookingId bookingId,
        Guid hotelId,
        Guid roomId,
        decimal totalPrice,
        string currency,
        DateTime checkInDate,
        DateTime checkOutDate,
        DateTime createdAt,
        BookingStatus status
)    {
        BookingId = bookingId;
        HotelId = hotelId;
        RoomId = roomId;
        TotalPrice = totalPrice;
        Currency = currency;
        CheckInDate = checkInDate.Date;
        CheckOutDate = checkOutDate.Date;
        CreatedAt = DateTime.UtcNow;
        Status = status;
    }

    public static Booking CreateNew(
        Guid hotelId,
        Guid roomId,
        DateTime checkIn,
        DateTime checkOut,
        decimal totalPrice,
        string currency)
    {
        var bookingId = new BookingId(Guid.NewGuid()); 
    
        var booking = new Booking(
            bookingId,
            hotelId,
            roomId, 
            totalPrice,
            currency,
            checkIn,
            checkOut,
            DateTime.UtcNow, 
            BookingStatus.Pending 
        );

       
        booking.AddDomainEvent(new BookingCreatedDomainEvent(
            booking.BookingId,
            booking.RoomId,
            booking.CheckInDate,
            booking.CheckOutDate,
            booking.TotalPrice,
            booking.Currency
        ));

        return booking;
    }

    public void Confirmed()
    {
        if (Status == BookingStatus.Pending)
        {
            Status = BookingStatus.Confirmed;
        }
        
        AddDomainEvent(new BookingConfirmedDomainEvent(
            BookingId.Value,
            RoomId,
            CheckInDate,
            CheckOutDate));
    }

    public void Pending()
    {
        Status = BookingStatus.Pending;
    }

    public void Confirm()
    {
        Status = BookingStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = BookingStatus.Canceled;
        AddDomainEvent(new BookingCanceledDomainEvent(BookingId, RoomId));
    }
}