using Accommodations.Application.Query.Shared;
using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Rooms.GetRoomDetails;

public class GetRoomDetailsQueryHandler : IRequestHandler<GetRoomDetailsQuery, RoomDetailsDto?>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetRoomDetailsQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RoomDetailsDto?> Handle(GetRoomDetailsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
            SELECT
                "RoomId", "HotelId", "RoomNumber", "Type", "Beds",
                "Capacity", "Description", "Status", "IsActive",
                "BasePrice_Amount" AS "BasePriceAmount", 
                "BasePrice_Currency" AS "BasePriceCurrency"
            FROM "Accommodations"."Rooms"
            WHERE "RoomId" = @RoomId
            """;

        var parameters = new
        {
            RoomId = request.Id
        };
        return await connection
            .QueryFirstOrDefaultAsync<RoomDetailsDto>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }
}
