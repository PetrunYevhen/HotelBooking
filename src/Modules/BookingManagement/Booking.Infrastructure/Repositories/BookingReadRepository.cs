using BookingManagement.Domain.Entities;
using BookingManagement.Domain.RepositoryContracts;
using Dapper;
using Infrastructure.Data;

namespace BookingManagement.Infrastructure.Repositories;

public class BookingReadRepository : IBookingReadRepository

{
private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
    public BookingReadRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task<List<Booking>> GetAllReservationsAsync(CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();

        
        const string request = "SELECT * FROM \"BookingManagement\".\"Bookings\""; 
        var reservations = await connection.QueryAsync<Booking>(request);
        return reservations.ToList();
    }
    
    public async  Task<Booking> GetByIdAsync
(BookingId bookingId, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();

        
        const string request = "SELECT * FROM \"BookingManagement\".\"Bookings\" WHERE \"BookingId\" = @BookingId";
        
        var booking = await connection.QueryFirstOrDefaultAsync<Booking>(
            new CommandDefinition(request, 
                new
                {
                    BookingId = bookingId.Value
                }, 
                cancellationToken: cancellationToken));
        
        return booking;
    }

    public async Task<bool> IsBookedInRangeAsync(Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();
        
        const string sql = @"
                            SELECT COUNT(*) 
                            FROM ""BookingManagement"".""Bookings"" 
                            WHERE ""RoomId"" = @RoomId 
                                AND ""Status"" = 0 
                                AND (
                                    (""CheckInDate"" <= @CheckOut 
                                         AND ""CheckOutDate"" >= @CheckIn)
                                    )";
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new 
        { 
            RoomId = roomId, 
            CheckIn = checkIn, 
            CheckOut = checkOut 
        });
        
        return count > 0;
    }   
}