using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Domain.RepositoryContracts;

namespace Booking.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    public async Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        Console.WriteLine("Reservation added to database.");
        await Task.CompletedTask;
    }

    public Task<Reservation> GetReservationByIdAsync(ReservationId requestReservationId, CancellationToken cancellationToken)
    {
        var reservation = new Reservation
        (
            requestReservationId,
            Guid.NewGuid(), // Simulating GuestId
            Guid.NewGuid(), // Simulating RoomId
            100, // Simulating Price
            DateTime.UtcNow, // Simulating StartDate
            DateTime.UtcNow.AddDays(1), // Simulating EndDate
            ReservationStatus.Pending // Simulating Status
        ); 
        return Task.FromResult(reservation);
    }
}