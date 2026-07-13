using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.GetHotelDetails;

public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, HotelDetailsDto?>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetHotelDetailsQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<HotelDetailsDto?> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT "Name", "Description", "Rating", 
                            "MinRoomPrice_Amount" AS "MinRoomPriceAmount",
                            "MinRoomPrice_Currency" AS "MinRoomPriceCurrency", 
                            "Country", "City", "Street", "PostalCode"
                            FROM "Accommodations"."Hotels"
                            WHERE "HotelId" = @HotelId
                           """;

        var parameters = new
        {
            HotelId = request.HotelId
        };

        return await connection
            .QueryFirstOrDefaultAsync<HotelDetailsDto>(new CommandDefinition
                (sql, parameters, cancellationToken: cancellationToken));
    }
}
