using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.HotelAddOns;

public sealed class GetHotelAddOnQueryHandler : IRequestHandler<GetHotelAddOnQuery, HotelAddOnDto?>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;
    public GetHotelAddOnQueryHandler(INpgsqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<HotelAddOnDto?> Handle(GetHotelAddOnQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();
        const string sql = """
            SELECT "HotelAddOnId", "HotelId", "Code", "Name", "Description",
                   "Price_Amount" AS "PriceAmount", "Price_Currency" AS "PriceCurrency",
                   "PricingType", "IsActive"
            FROM "Accommodations"."HotelAddOns"
            WHERE "HotelId" = @HotelId AND "HotelAddOnId" = @HotelAddOnId
            """;
        var row = await connection.QuerySingleOrDefaultAsync<HotelAddOnRow>(new CommandDefinition(sql,
            new { request.HotelId, request.HotelAddOnId }, cancellationToken: cancellationToken));
        return row is null ? null : GetHotelAddOnsQueryHandler.Map(row);
    }
}
