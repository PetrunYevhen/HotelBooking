using BookingManagement.Domain.RepositoryContracts;

namespace BookingManagement.Infrastructure.Repositories;

public class ReservationWriteRepository : IReservationWriteRepository
{
    private readonly BookingDbContext _bookingDbContext;
    
    public ReservationWriteRepository(BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
    }
    public async Task AddReservationAsync(Domain.Entities.Booking booking, CancellationToken cancellationToken)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking), "Reservation cannot be null.");
        
        
        await _bookingDbContext.Reservations.AddAsync(booking, cancellationToken);
        await _bookingDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateReservationAsync(Domain.Entities.Booking booking, CancellationToken cancellationToken)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking), "Reservation cannot be null.");
        
        _bookingDbContext.Reservations.Update(booking);
        return _bookingDbContext.SaveChangesAsync(cancellationToken);
    }
}