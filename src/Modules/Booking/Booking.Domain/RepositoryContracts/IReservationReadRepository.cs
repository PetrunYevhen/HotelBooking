using Booking.Domain.Entities;

namespace Booking.Domain.RepositoryContracts;

public interface IReservationReadRepository
{
    Task<List<Reservation>> GetAllReservationsAsync(CancellationToken cancellationToken);
    Task<Reservation> GetReservationByIdAsync(ReservationId reservationId, CancellationToken cancellationToken);
}