using Dapper;
using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using Infrastructure.Data;

namespace HotelManagement.Infastructure.Repositories;

public class HotelReadRepository : IHotelReadRepository
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;
    
    public HotelReadRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }
    public async Task<Hotel> GetHotelByIdAsync(HotelId hotelId, CancellationToken cancellationToken)
    { 
        using var connection = _npgsqlConnectionFactory.CreateConnection();
    
        const string sql = "SELECT * FROM \"HotelManagement\".\"Hotels\" WHERE \"HotelId\" = @HotelId";
    
        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { HotelId = hotelId },
            cancellationToken: cancellationToken);
    
        var result = await connection.QueryFirstOrDefaultAsync<Hotel>(command);

        if (result == null)
        {
            throw new Exception($"Hotel not found with id {hotelId}");
        }

        return result;
    }

    // public async Task<List<Guid>> GetRoomIdsByHotelIdAsync(HotelId hotelId, CancellationToken cancellationToken)
    // {
    //     using var connection = _npgsqlConnectionFactory.CreateConnection(); 
    //     
    //     const string sql = "SELECT \"RoomId\" FROM \"Shared\".\"HotelRooms\" WHERE \"HotelId\" = @HotelId";
    //     var command = new CommandDefinition(
    //         commandText: sql,
    //         parameters: new { HotelId = hotelId.Value },
    //         cancellationToken: cancellationToken);
    //     
    //     var result = await connection.QueryAsync<Guid>(command);
    //     
    //     return result.ToList();
    // }

    // public async Task<List<Guid>> GetFacilityIdsByHotelIdAsync(HotelId hotelId, CancellationToken cancellationToken)
    // {
    //     using var connection = _npgsqlConnectionFactory.CreateConnection(); 
    //     const string sql = "SELECT \"FacilityId\" FROM \"Shared\".\"HotelFacilities\" WHERE \"HotelId\" = @HotelId";
    //     
    //     var command = new CommandDefinition(
    //         commandText: sql,
    //         parameters: new { HotelId = hotelId.Value },
    //         cancellationToken: cancellationToken);
    //     
    //     var result = await connection.QueryAsync<Guid>(command);
    //     return result.ToList();
    // }
}