using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.GetCheckOutHours;

public class GetHotelCheckOutHoursQueryHandler : IRequestHandler<GetHotelCheckOutHoursQuery, int>
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

    public GetHotelCheckOutHoursQueryHandler(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task<int> Handle(GetHotelCheckOutHoursQuery request, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateNewConnection();

        const string sql = """
                           SELECT "CheckOutHoursPolicy"
                           FROM "Accommodations"."Hotels"
                           WHERE "HotelId" = @HotelId
                           """;

        return await connection.QueryFirstOrDefaultAsync<int>(new CommandDefinition(
            sql, new { request.HotelId }, cancellationToken: cancellationToken));
    }
}