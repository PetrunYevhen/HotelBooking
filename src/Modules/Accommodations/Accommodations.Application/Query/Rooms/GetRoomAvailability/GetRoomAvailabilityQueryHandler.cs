using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Rooms.GetRoomAvailability;

public class GetRoomAvailabilityQueryHandler : IRequestHandler<GetRoomAvailabilityQuery, bool>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetRoomAvailabilityQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> Handle(GetRoomAvailabilityQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT "IsActive" 
                            FROM "Accommodations"."Rooms"
                            WHERE "RoomId" = @RoomId;
                           """;

        var parameters = new
        {
            RoomId = request.RoomId
        };
        
        return await connection.QueryFirstOrDefaultAsync<bool>(
            new CommandDefinition
                (sql, parameters, cancellationToken: cancellationToken) ) ;
    }
}