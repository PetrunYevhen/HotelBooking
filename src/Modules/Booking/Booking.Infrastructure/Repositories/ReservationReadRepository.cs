using System.Data;
using Booking.Domain.Entities;
using Booking.Domain.RepositoryContracts;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Booking.Infrastructure.Repositories;

public class ReservationReadRepository : IReservationReadRepository
{
    private readonly IDbConnection _connection;
    
    public ReservationReadRepository(IConfiguration configuration)
    {
       var connectionString = configuration.GetConnectionString("DefaultConnection");
        _connection =  new Npgsql.NpgsqlConnection(connectionString);
    }

    public async Task<List<Reservation>> GetAllReservationsAsync(CancellationToken cancellationToken)
    {
        const string request = "SELECT * FROM \"Bookings\".\"Bookings\""; 
        var reservations = await _connection.QueryAsync<Reservation>(request);
        return reservations.ToList();
    }
    
    public async  Task<Reservation> GetReservationByIdAsync(ReservationId reservationId, CancellationToken cancellationToken)
    {
        const string request = "SELECT * FROM \"Bookings\".\"Bookings\" WHERE \"ReservationId\" = @ReservationId";
        var reservation = await _connection.QueryFirstOrDefaultAsync<Reservation>(
            new CommandDefinition(request, new { ReservationId = reservationId.Value }, cancellationToken: cancellationToken));
        
        if (reservation is null) 
            throw new KeyNotFoundException($"Reservation with ID {reservationId.Value} not found.");
        
        return reservation;
    }
}