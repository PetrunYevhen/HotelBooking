using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Bookings.Application.Query.GetBookingById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, BookingDto>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetByIdQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<BookingDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
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
                            WHERE "BookingId" = @BookingId
                            """;

        return await connection
            .QueryFirstOrDefaultAsync<BookingDto>(new CommandDefinition(sql, new { request.BookingId }, cancellationToken: cancellationToken));
    }
}
