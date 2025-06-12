using Booking.Domain.Entities;

namespace Booking.Domain.RepositoryContracts;

public interface IReservationWriteRepository 
{
    Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken);
}