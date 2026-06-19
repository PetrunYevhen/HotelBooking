using Accommodations.Application.Query.Shared;
using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Rooms.GetRoomsByHotelId;

public class GetRoomsByHotelIdQueryHandler : IRequestHandler<GetRoomsByHotelIdQuery, List<RoomDetailsDto>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetRoomsByHotelIdQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<RoomDetailsDto>> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                           SELECT
                               r."RoomId", r."HotelId", r."RoomNumber", r."Type", r."Beds",
                               r."Capacity", r."Description", r."Status", r."IsActive",
                               "BasePrice_Amount" AS "BasePriceAmount",
                               "BasePrice_Currency" AS "BasePriceCurrency",
                               COALESCE(p."Price_Amount", r."BasePrice_Amount") AS "EffectivePriceAmount",
                           COALESCE(p."Price_Currency", r."BasePrice_Currency") AS "EffectivePriceCurrency"
                           FROM "Accommodations"."Rooms" r
                           LEFT JOIN LATERAL ( 
                              SELECT "Price_Amount", "Price_Currency"
                              FROM "Accommodations"."Pricing"
                              WHERE "RoomId" = r."RoomId"
                              AND "ValidFrom" <= @CheckIn AND "ValidTo" > @CheckIn
                              AND "IsActive" = true
                              ORDER BY "Type" DESC
                              LIMIT 1
                           ) p ON true
                           WHERE "HotelId" = @HotelId AND "IsActive" = true
                           ORDER BY "RoomNumber"
                           """;

        var parameters = new
        {
            HotelId = request.HotelId,
            CheckIn = request.CheckIn
        };

        var query = await connection
            .QueryAsync<RoomDetailsDto>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        
        return query.ToList();
    }
}