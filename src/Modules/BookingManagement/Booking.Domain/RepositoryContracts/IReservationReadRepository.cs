using BookingManagement.Domain.Entities;

namespace BookingManagement.Domain.RepositoryContracts;

public interface IReservationReadRepository
{
    Task<List<Booking>> GetAllReservationsAsync(CancellationToken cancellationToken);
    Task<Booking> GetReservationByIdAsync(BookingId bookingId, CancellationToken cancellationToken);
    Task<bool> CheckRoomAvailabilityAsync(Guid roomId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
}