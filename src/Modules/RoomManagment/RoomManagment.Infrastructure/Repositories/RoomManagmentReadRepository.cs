using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using RoomManagment.Domain.Entities;
using RoomManagment.Domain.RepositoryContract;
using RoomManagment.Infrastructure.Data;

namespace RoomManagment.Infrastructure.Repositories;

public class RoomManagmentReadRepository : IRoomManagmentReadRepository
{
    private readonly IDbConnection _connection;

    public RoomManagmentReadRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        _connection = new Npgsql.NpgsqlConnection(connectionString);
    }


    public async Task<Room> GetRoomByIdAsync(RoomId roomId)
    {
        const string query = "SELECT * FROM \"Rooms\".\"Rooms\" WHERE \"RoomId\" = @RoomId";

        return await _connection.QueryFirstOrDefaultAsync<Room>(
            new CommandDefinition(query, new { RoomId = roomId.Value }));
    }
}