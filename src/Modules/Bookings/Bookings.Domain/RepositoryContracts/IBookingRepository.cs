using Bookings.Domain.Entities;
using SharedKernel.ValueObjects;

namespace Bookings.Domain.RepositoryContracts;

public interface IBookingRepository
{
    Task<Booking> GetByIdAsync(BookingId id,  CancellationToken cancellationToken);
    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    Task UpdateAsync (Booking booking, CancellationToken cancellationToken);
    Task DeleteAsync(Booking booking, CancellationToken cancellationToken);
    Task<bool> HasOverlappingBookingAsync(Guid roomId, DateRange bookingDates, CancellationToken cancellationToken);
}