using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.GetHotelsByOwner;

public sealed class GetHotelsByOwnerQueryHandler(INpgsqlConnectionFactory connectionFactory) : IRequestHandler<GetHotelsByOwnerQuery, List<HotelierHotelDto>>
{
    public async Task<List<HotelierHotelDto>> Handle(GetHotelsByOwnerQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateNewConnection();
        const string sql = """
            SELECT "HotelId", "Name", "City", "Country", "OwnerUserId"
            FROM "Accommodations"."Hotels"
            WHERE "OwnerUserId" = @OwnerUserId OR @IncludeAll = true
            ORDER BY "Name"
            """;
        return (await connection.QueryAsync<HotelierHotelDto>(new CommandDefinition(sql,
            new { request.OwnerUserId, request.IncludeAll }, cancellationToken: cancellationToken))).ToList();
    }
}
