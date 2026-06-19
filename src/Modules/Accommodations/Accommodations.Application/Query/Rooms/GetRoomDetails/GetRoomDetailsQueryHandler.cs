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
                r."RoomId", r."HotelId", r."RoomNumber", r."Type", r."Beds",
                r."Capacity", r."Description", r."Status", r."IsActive",
                r."BasePrice_Amount" AS "BasePriceAmount", 
                r."BasePrice_Currency" AS "BasePriceCurrency",
                COALESCE(p."Price_Amount", r."BasePrice_Amount") AS "EffectivePriceAmount",
                COALESCE(p."Price_Currency", r."BasePrice_Currency") AS "EffectivePriceCurrency"
            FROM "Accommodations"."Rooms" r
            LEFT JOIN LATERAL ( 
                SELECT "Price_Amount", "Price_Currency"
                FROM "Accommodations"."Pricing"
                WHERE "RoomId" = r."RoomId"
                AND "ValidFrom" <= @CheckIn AND "ValidTo" >= @CheckIn
                AND "IsActive" = true
                ORDER BY "Type" DESC
                LIMIT 1
             ) p ON true
             WHERE r."RoomId" = @RoomId AND r."IsActive" = true
             ORDER BY "RoomNumber" DESC
            """;

        var parameters = new
        {
            RoomId = request.Id,
            CheckIn = request.CheckIn,
        };
        return await connection
            .QueryFirstOrDefaultAsync<RoomDetailsDto>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }
}
