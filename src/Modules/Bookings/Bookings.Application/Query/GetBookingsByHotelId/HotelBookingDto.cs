namespace Bookings.Application.Query.GetBookingsByHotelId;

public sealed record HotelBookingDto(Guid Id, Guid HotelId, Guid RoomId, string RoomNumber, string GuestName,
    string GuestEmail, DateTime CheckInDate, DateTime CheckOutDate, decimal TotalPrice, string Currency,
    int GuestsCount, string Status, DateTime CreatedAt);
