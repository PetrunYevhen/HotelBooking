using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.GetAllHotels;

public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, List<HotelDto>>
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

    public GetAllHotelsQueryHandler(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task<List<HotelDto>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT "HotelId", "Name", "City", "Country",
                            "Rating", 
                            "MinRoomPrice_Amount" AS MinRoomPriceAmount,
                            "MinRoomPrice_Currency" AS MinRoomPriceCurrency
                            FROM "Accommodations"."Hotels"
                            LIMIT 50
                           """;
        
        var query = await connection.QueryAsync<HotelDto>(
            new CommandDefinition(sql, cancellationToken));
        
        return query.ToList();
    }
}