namespace Bookings.Application.Query.GetBookingById;

public record BookingDto(
    Guid Id,
    Guid RoomId,
    Guid HotelId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice,
    string Currency,
    string Status,
    DateTime CreatedAt
);
    
