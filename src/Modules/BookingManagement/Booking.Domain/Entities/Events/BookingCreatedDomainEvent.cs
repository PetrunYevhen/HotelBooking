using BuildingBlock.Domain.Events;

namespace BookingManagement.Domain.Entities.Events;

public class BookingCreatedDomainEvent : DomainEventBase
{
    public BookingId BookingId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    
    // Додаємо ці дані тут
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }

    public BookingCreatedDomainEvent(
        BookingId bookingId, 
        Guid roomId, 
        DateTime checkInDate, 
        DateTime checkOutDate,
        decimal totalPrice,   // Новий параметр
        string currency)      // Новий параметр
    {
        BookingId = bookingId;
        RoomId = roomId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        TotalPrice = totalPrice;
        Currency = currency;
    }
}