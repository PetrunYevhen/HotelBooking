using Bookings.Domain.Entities;

namespace Bookings.Domain.RepositoryContracts;

public interface IBookingReadRepository

{
    Task<List<Booking>> GetAllReservationsAsync(CancellationToken cancellationToken);
    Task<Booking> GetByIdAsync
(BookingId bookingId, CancellationToken cancellationToken);
    Task<bool> IsBookedInRangeAsync(Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken);
}