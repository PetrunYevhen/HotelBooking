using Accommodations.Application.Query.Rooms.GetRoomDetails;
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
                               "RoomId", "HotelId", "RoomNumber", "Type", "Beds",
                               "Capacity", "Description", "Status", "IsActive",
                               "BasePrice_Amount" AS "BasePriceAmount",
                               "BasePrice_Currency" AS "BasePriceCurrency"
                           FROM "Accommodations"."Rooms"
                           WHERE "HotelId" = @HotelId AND "IsActive" = true
                           ORDER BY "RoomNumber"
                           """;

        var parameters = new
        {
            HotelId = request.HotelId,
        };

        var query = await connection
            .QueryAsync<RoomDetailsDto>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        
        return query.ToList();
    }
}