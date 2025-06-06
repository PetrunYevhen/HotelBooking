namespace Booking.IntegrationEvents;

public record AddNewReservationIntegrationEvent(
    Guid ReservationId,
    Guid GuestId,
    Guid RoomId,
    decimal Price,
    DateTime StartDate,
    DateTime EndDate,
    string Status
    );