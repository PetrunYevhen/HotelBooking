using Booking.Domain.Entities;

namespace Booking.Domain.RepositoryContracts;

public interface IReservationRepository
{
    Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken);
    Task<Reservation> GetReservationByIdAsync(ReservationId requestReservationId, CancellationToken cancellationToken);
}