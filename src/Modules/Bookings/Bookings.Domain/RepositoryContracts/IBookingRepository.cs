using Bookings.Domain.Entities;

namespace Bookings.Domain.RepositoryContracts;

public interface IBookingWriteRepository
{
    Task CreateAsync(Booking booking, CancellationToken cancellationToken);
    Task UpdateAsync (Booking booking, CancellationToken cancellationToken);
    Task DeleteAsync(Booking booking, CancellationToken cancellationToken);
}