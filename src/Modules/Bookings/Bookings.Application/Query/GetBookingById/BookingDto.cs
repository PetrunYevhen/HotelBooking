namespace Bookings.Application.Query.GetBookingById;

public record BookingDto(
    Guid Id,
    Guid RoomId,
    Guid HotelId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice,
    string Currency,
    int GuestsCount,
    string Status,
    string? CompletionReason,
    DateTime CreatedAt,
    IReadOnlyList<BookingAddOnDto>? AddOns = null
)
{
    // Dapper's booking list query does not select add-ons; keep a matching constructor.
    public BookingDto(Guid id, Guid roomId, Guid hotelId, DateTime checkInDate, DateTime checkOutDate,
        decimal totalPrice, string currency, int guestsCount, string status,
        string? completionReason, DateTime createdAt)
        : this(id, roomId, hotelId, checkInDate, checkOutDate, totalPrice, currency,
            guestsCount, status, completionReason, createdAt, null)
    {
    }
}

public record BookingAddOnDto(
    string Code,
    string Name,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    string Currency);
    
