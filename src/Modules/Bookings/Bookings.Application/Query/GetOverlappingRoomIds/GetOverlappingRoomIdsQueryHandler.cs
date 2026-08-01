using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Bookings.Application.Query.GetOverlappingRoomIds;

public class GetOverlappingRoomIdsQueryHandler : IRequestHandler<GetOverlappingRoomIdsQuery, List<Guid>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetOverlappingRoomIdsQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Guid>> Handle(GetOverlappingRoomIdsQuery request, CancellationToken cancellationToken)
    {
        if (request.RoomIds.Count == 0)
            return new List<Guid>();

        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT DISTINCT "RoomId"
                            FROM "Bookings"."Bookings"
                            WHERE "RoomId" = ANY(@RoomIds) AND "Status" != 3
                              AND "CheckIn" < @CheckOut AND "CheckOut" > @CheckIn
                            """;

        var parameters = new
        {
            RoomIds = request.RoomIds,
            request.CheckIn,
            request.CheckOut
        };

        var result = await connection.QueryAsync<Guid>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        return result.ToList();
    }
}
