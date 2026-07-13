using Bookings.Application.Query.GetBookingById;
using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Bookings.Application.Query.GetBookingsByUserId;

public class GetBookingsByUserIdQueryHandler : IRequestHandler<GetBookingsByUserIdQuery, List<BookingDto>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetBookingsByUserIdQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<BookingDto>> Handle(GetBookingsByUserIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT
                                "BookingId" AS "Id",
                                "RoomId",
                                "HotelId",
                                "CheckIn" AS "CheckInDate",
                                "CheckOut" AS "CheckOutDate",
                                "TotalPrice_Amount" AS "TotalPrice",
                                "TotalPrice_Currency" AS "Currency",
                                CASE "Status"
                                    WHEN 0 THEN 'Pending'
                                    WHEN 1 THEN 'Confirmed'
                                    WHEN 2 THEN 'Completed'
                                    WHEN 3 THEN 'Cancelled'
                                    WHEN 4 THEN 'CheckedIn'
                                END AS "Status",
                                "CreatedAt"
                            FROM "Bookings"."Bookings"
                            WHERE "UserId" = @UserId
                            ORDER BY "CreatedAt" DESC
                            """;

        var query = await connection
            .QueryAsync<BookingDto>(new CommandDefinition(sql, new { request.UserId }, cancellationToken: cancellationToken));

        return query.ToList();
    }
}
