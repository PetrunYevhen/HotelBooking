using Dapper;
using Infrastructure.Data;
using RoomManagment.Domain.Entities;
using RoomManagment.Domain.RepositoryContract;

namespace RoomManagment.Infrastructure.Repositories;

public class RoomManagmentReadRepository : IRoomManagmentReadRepository
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

    public RoomManagmentReadRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }


    public async Task<Room> GetRoomByIdAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();
        
        const string query = "SELECT * FROM \"RoomManagement\".\"Rooms\" WHERE \"RoomId\" = @RoomId";

        var command = new CommandDefinition(
            commandText: query, 
            parameters: new { RoomId = roomId.Value },
            cancellationToken: cancellationToken);
        
        var result = await connection.QueryFirstOrDefaultAsync<Room>(
           command);
        
        return result;
    }

    public async Task<decimal> GetMinRoomPriceInHotelAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();
        const string sql = @"
                            SELECT MIN(r.""PricePerNight"")
                            FROM ""RoomManagement"".""Rooms"" r
                            JOIN ""Shared"".""HotelRooms"" hr
                              ON r.""RoomId"" = hr.""RoomId""
                            WHERE hr.""HotelId"" = @HotelId;";     
        
        var command = new CommandDefinition(
            commandText: sql, 
            parameters: new { HotelId = hotelId },
            cancellationToken: cancellationToken);
        var result = await connection.ExecuteScalarAsync<decimal>(command);
        
        return result;
    }

    public async Task<decimal> GetPriceForRoomAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();
        
        const string sql = @"
                        SELECT r.""PricePerNight""
                        FROM ""RoomManagement"".""Rooms"" r
                        WHERE r.""RoomId"" = @RoomId;";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { RoomId = roomId.Value },
            cancellationToken: cancellationToken);
        
        var result = await connection.ExecuteScalarAsync<int>(command);

        return result;
    }

    public async Task<bool> IsRoomAvailableAsync(RoomId roomId, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();

        const string sql = @"
                        SELECT COUNT(1)
                        FROM ""RoomManagement"".""Rooms""
                        WHERE ""Id"" = @RoomId
                          AND ""Status"" = 'Free'";
        
        var command = new CommandDefinition(
            commandText: sql, 
            parameters: new { RoomId = roomId.Value },
            cancellationToken: cancellationToken);
        
        var result = await connection.ExecuteScalarAsync<int>(command);
        
        return result > 0;
    }
}