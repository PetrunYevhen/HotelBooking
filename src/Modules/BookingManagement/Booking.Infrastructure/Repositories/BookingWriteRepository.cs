using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;

namespace BookingManagement.Infrastructure.Repositories;

public class BookingWriteRepository : IBookingWriteRepository
{
    private readonly BookingDbContext _bookingDbContext;
    
    public BookingWriteRepository (BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
    }
    public async Task CreateAsync
        (Booking booking, CancellationToken cancellationToken)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking), "Reservation cannot be null.");
        
        
        await _bookingDbContext.Bookings .AddAsync(booking, cancellationToken);
        await _bookingDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync (Booking booking, CancellationToken cancellationToken)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking), "Booking cannot be null.");
        
        _bookingDbContext.Bookings .Update(booking);
        return _bookingDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(Booking booking, CancellationToken cancellationToken)
    {
        _bookingDbContext.Bookings .Remove(booking);
        return _bookingDbContext.SaveChangesAsync(cancellationToken);
    }
}