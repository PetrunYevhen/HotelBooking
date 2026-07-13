using Bookings.Domain.Entities;
using Bookings.Domain.Entities.Enums;
using Bookings.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using SharedKernel.ValueObjects;

namespace Bookings.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _bookingDbContext;
    private IBookingRepository _bookingRepositoryImplementation;

    public BookingRepository (BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
    }

    public async Task<Booking> GetByIdAsync(BookingId id, CancellationToken cancellationToken)
    {
        return await _bookingDbContext.Bookings.FirstOrDefaultAsync(b => b.BookingId == id, cancellationToken);
    }

    public async Task AddAsync
        (Booking booking, CancellationToken cancellationToken)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking), "Reservation cannot be null.");
        
        await _bookingDbContext.Bookings.AddAsync(booking, cancellationToken);
    }

    public Task UpdateAsync (Booking booking, CancellationToken cancellationToken)
    {
        if(booking is null)
            throw new ArgumentNullException(nameof(booking), "Booking cannot be null.");
        
        _bookingDbContext.Bookings.Update(booking);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Booking booking, CancellationToken cancellationToken)
    {
        _bookingDbContext.Bookings.Remove(booking);
        return Task.CompletedTask;
    }

    public async Task<bool> HasOverlappingBookingAsync(Guid roomId, DateRange bookingDates, CancellationToken cancellationToken)
    {
        return await _bookingDbContext.Bookings
            .AnyAsync(b =>
                    b.RoomId == roomId &&
                    b.Status != BookingStatus.Cancelled &&
                    b.BookingDates.Start < bookingDates.End &&
                    b.BookingDates.End > bookingDates.Start,
                cancellationToken);
    }

    public async Task<List<Booking>> GetOverdueCheckedInBookingsAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        
        return await _bookingDbContext.Bookings
            .Where(b => b.Status == BookingStatus.CheckedIn && b.BookingDates.End < now)
            .ToListAsync(cancellationToken);
    }
}