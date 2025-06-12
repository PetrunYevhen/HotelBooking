using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Domain.RepositoryContracts;
using Booking.Infrastructure.Data;
using MediatR;

namespace Booking.Infrastructure.Repositories;

public class ReservationWriteRepository : IReservationWriteRepository
{
    private readonly BookingDbContext _bookingDbContext;
    
    public ReservationWriteRepository(BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
    }
    public async Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        if (reservation is null)
            throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null.");
        
        
        await _bookingDbContext.Reservations.AddAsync(reservation, cancellationToken);
        await _bookingDbContext.SaveChangesAsync(cancellationToken);
    }

    
}