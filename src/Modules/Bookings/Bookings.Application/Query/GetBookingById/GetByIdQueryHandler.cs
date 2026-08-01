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
                                "GuestsCount",
                                CASE "Status"
                                    WHEN 0 THEN 'Pending'
                                    WHEN 1 THEN 'Confirmed'
                                    WHEN 2 THEN 'Completed'
                                    WHEN 3 THEN 'Cancelled'
                                    WHEN 4 THEN 'CheckedIn'
                                    WHEN 5 THEN 'NoShow'
                                END AS "Status",
                                CASE "CompletionReason"
                                    WHEN 1 THEN 'StaffCheckout'
                                    WHEN 2 THEN 'AutomaticCheckout'
                                END AS "CompletionReason",
                                "CreatedAt"
                            FROM "Bookings"."Bookings"
                            WHERE "BookingId" = @BookingId
                            """;

        var booking = await connection
            .QueryFirstOrDefaultAsync<BookingDto>(new CommandDefinition(sql, new { request.BookingId }, cancellationToken: cancellationToken));
        if (booking is null)
            return null;

        const string addOnsSql = """
                                  SELECT
                                      "Code",
                                      "Name",
                                      "Quantity",
                                      "UnitPrice_Amount" AS "UnitPrice",
                                      "TotalPrice_Amount" AS "TotalPrice",
                                      "TotalPrice_Currency" AS "Currency"
                                  FROM "Bookings"."BookingAddOns"
                                  WHERE "BookingId" = @BookingId
                                  ORDER BY "Name"
                                  """;
        var addOns = await connection.QueryAsync<BookingAddOnDto>(
            new CommandDefinition(addOnsSql, new { request.BookingId }, cancellationToken: cancellationToken));

        return booking with { AddOns = addOns.ToList() };
    }
}
