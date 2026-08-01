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
                            WHERE "UserId" = @UserId
                            ORDER BY "CreatedAt" DESC
                            """;

        var bookings = (await connection
            .QueryAsync<BookingDto>(new CommandDefinition(sql, new { request.UserId }, cancellationToken: cancellationToken)))
            .ToList();

        if (bookings.Count == 0)
            return bookings;

        const string addOnsSql = """
                                  SELECT
                                      "BookingId",
                                      "Code",
                                      "Name",
                                      "Quantity",
                                      "UnitPrice_Amount" AS "UnitPrice",
                                      "TotalPrice_Amount" AS "TotalPrice",
                                      "TotalPrice_Currency" AS "Currency"
                                  FROM "Bookings"."BookingAddOns"
                                  WHERE "BookingId" = ANY(@BookingIds)
                                  ORDER BY "Name"
                                  """;
        var addOns = await connection.QueryAsync<BookingAddOnRow>(new CommandDefinition(
            addOnsSql,
            new { BookingIds = bookings.Select(booking => booking.Id).ToArray() },
            cancellationToken: cancellationToken));
        var addOnsByBooking = addOns.GroupBy(addOn => addOn.BookingId).ToDictionary(
            group => group.Key,
            group => (IReadOnlyList<BookingAddOnDto>)group.Select(addOn => new BookingAddOnDto(addOn.Code, addOn.Name, addOn.Quantity, addOn.UnitPrice, addOn.TotalPrice, addOn.Currency)).ToList());

        return bookings.Select(booking => booking with
        {
            AddOns = addOnsByBooking.GetValueOrDefault(booking.Id, [])
        }).ToList();
    }

    private sealed record BookingAddOnRow(Guid BookingId, string Code, string Name, int Quantity, decimal UnitPrice, decimal TotalPrice, string Currency);
}
