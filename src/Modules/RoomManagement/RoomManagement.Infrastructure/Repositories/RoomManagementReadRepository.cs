using Dapper;
using Infrastructure.Data;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Infrastructure.Repositories;

public class RoomManagementReadRepository : IRoomManagementReadRepository
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

    public RoomManagementReadRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }


    public async Task<Room> GetByIdAsync(RoomId roomId,  CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();
        
        const string sql = @"SELECT * FROM ""RoomManagement"".""Rooms""
                             WHERE ""RoomId"" = @RoomId;";
        
        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { RoomId = roomId },
            cancellationToken: cancellationToken);
        
        var result = await connection.QuerySingleOrDefaultAsync<Room>(command);
        return result;
    }


    public async Task<decimal> GetMinPriceAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();
        
        const string sql = @"
                        SELECT MAX(""PricePerNight"")
                        FROM ""RoomManagement"".""Rooms"" 
                        WHERE ""HotelId"" = @HotelId;";

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { HotelId = hotelId },
            cancellationToken: cancellationToken);
        
        var result = await connection.ExecuteScalarAsync<decimal>(command);

        return result;
    }

    public async Task<List<Room>> GetByHotelIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateConnection();

        const string sql = @"SELECT * FROM ""RoomManagement"".""Rooms""
                             WHERE ""HotelId"" = @HotelId;";
        
        var command = new CommandDefinition(
            commandText: sql, 
            parameters: new { HotelId = id },
            cancellationToken: cancellationToken);
        
        var result = await connection.QueryAsync<Room>(command);
        return result.ToList();
    }

    public async Task<bool> IsRoomAvailableAsync(RoomId roomId,
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