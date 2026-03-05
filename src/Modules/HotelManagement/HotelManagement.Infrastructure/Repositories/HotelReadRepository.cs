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
    public async Task<Hotel> GetByHotelIdAsync(HotelId id, CancellationToken cancellationToken)
    { 
        using var connection = _npgsqlConnectionFactory.CreateConnection();
    
        const string sql = "SELECT * FROM \"HotelManagement\".\"Hotels\" WHERE \"HotelId\" = @HotelId";
    
        var command = new CommandDefinition(
            commandText: sql,
            parameters: new { HotelId = id.Value },
            cancellationToken: cancellationToken);
    
        var result = await connection.QueryFirstOrDefaultAsync<Hotel>(command);

        if (result == null)
        {
            throw new Exception($"Hotel not found with id {id}");
        }

        return result;
    }

    // public async Task<List<Guid>> GetRoomIdsByIdAsync(Id Id, CancellationToken cancellationToken)
    // {
    //     using var connection = _npgsqlConnectionFactory.CreateConnection(); 
    //     
    //     const string sql = "SELECT \"RoomId\" FROM \"Shared\".\"HotelRooms\" WHERE \"Id\" = @Id";
    //     var command = new CommandDefinition(
    //         commandText: sql,
    //         parameters: new { Id = Id.Value },
    //         cancellationToken: cancellationToken);
    //     
    //     var result = await connection.QueryAsync<Guid>(command);
    //     
    //     return result.ToList();
    // }

    // public async Task<List<Guid>> GetFacilityIdsByIdAsync(Id Id, CancellationToken cancellationToken)
    // {
    //     using var connection = _npgsqlConnectionFactory.CreateConnection(); 
    //     const string sql = "SELECT \"FacilityId\" FROM \"Shared\".\"HotelFacilities\" WHERE \"Id\" = @Id";
    //     
    //     var command = new CommandDefinition(
    //         commandText: sql,
    //         parameters: new { Id = Id.Value },
    //         cancellationToken: cancellationToken);
    //     
    //     var result = await connection.QueryAsync<Guid>(command);
    //     return result.ToList();
    // }
}