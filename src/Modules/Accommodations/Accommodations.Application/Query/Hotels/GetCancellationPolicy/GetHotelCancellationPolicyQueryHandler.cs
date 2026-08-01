using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.GetCancellationPolicy;

public class GetHotelCancellationPolicyQueryHandler : IRequestHandler<GetHotelCancellationPolicyQuery, HotelCancellationPolicyDto>
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

    public GetHotelCancellationPolicyQueryHandler(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task<HotelCancellationPolicyDto> Handle(GetHotelCancellationPolicyQuery request, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateNewConnection();

        const string sql = """
                           SELECT "Policies_CancellationType" AS "Type",
                                  "Policies_Cancellation_DeadlineDays" AS "DeadlineDays",
                                  "Policies_Cancellation_PenaltyPercentage" AS "PercentagePenalty"
                           FROM "Accommodations"."Hotels"
                           WHERE "HotelId" = @HotelId
                           """;

        return await connection.QueryFirstOrDefaultAsync<HotelCancellationPolicyDto>(new CommandDefinition(
            sql, new { request.HotelId }, cancellationToken: cancellationToken));
    }
}
