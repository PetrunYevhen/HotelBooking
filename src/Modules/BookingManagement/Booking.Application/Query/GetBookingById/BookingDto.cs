namespace BookingManagement.Application.Query.GetBookingById;

public record BookingDto(
    Guid Id,
    Guid RoomId,
    Guid HotelId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt
);
    
