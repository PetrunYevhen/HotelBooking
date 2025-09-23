using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using Dapper;
using Infrastructure.Data;

namespace BookingManagement.Infrastructure.Repositories;

public class ReservationReadRepository : IReservationReadRepository
{
private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
    public ReservationReadRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task<List<Domain.Entities.Booking>> GetAllReservationsAsync(CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();

        
        const string request = "SELECT * FROM \"BookingManagement\".\"Bookings\""; 
        var reservations = await connection.QueryAsync<Domain.Entities.Booking>(request);
        return reservations.ToList();
    }
    
    public async  Task<Domain.Entities.Booking> GetReservationByIdAsync(BookingId bookingId, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();

        
        const string request = "SELECT * FROM \"BookingManagement\".\"Bookings\" WHERE \"BookingId\" = @BookingId";
        
        var booking = await connection.QueryFirstOrDefaultAsync<Domain.Entities.Booking>(
            new CommandDefinition(request, 
                new
                {
                    BookingId = bookingId.Value
                }, 
                cancellationToken: cancellationToken));
        
        return booking;
    }

    public async Task<bool> CheckRoomAvailabilityAsync(Guid roomId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();
        
        const string sql = @"
            SELECT COUNT(*) 
            FROM ""BookingManagement"".""Bookings"" 
            WHERE ""RoomId"" = @RoomId 
              AND ""Status"" = 1 
              AND (
                    (""StartDate"" <= @EndDate AND ""EndDate"" >= @StartDate)
                  )";
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new 
            { 
                RoomId = roomId, 
                StartDate = startDate, 
                EndDate = endDate 
            });
        
        return count == 0;
    }
}