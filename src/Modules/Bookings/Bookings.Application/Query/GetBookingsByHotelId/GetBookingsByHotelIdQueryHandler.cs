using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Bookings.Application.Query.GetBookingsByHotelId;

public sealed class GetBookingsByHotelIdQueryHandler(INpgsqlConnectionFactory connectionFactory) : IRequestHandler<GetBookingsByHotelIdQuery, List<HotelBookingDto>>
{
    public async Task<List<HotelBookingDto>> Handle(GetBookingsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateNewConnection();
        const string sql = """
            SELECT b."BookingId" AS "Id", b."HotelId", b."RoomId", '' AS "RoomNumber",
              b."GuestFirstName" || ' ' || b."GuestLastName" AS "GuestName", b."GuestEmail" AS "GuestEmail",
              b."CheckIn" AS "CheckInDate", b."CheckOut" AS "CheckOutDate", b."TotalPrice_Amount" AS "TotalPrice",
              b."TotalPrice_Currency" AS "Currency", b."GuestsCount",
              CASE b."Status" WHEN 0 THEN 'Pending' WHEN 1 THEN 'Confirmed' WHEN 2 THEN 'Completed'
                WHEN 3 THEN 'Cancelled' WHEN 4 THEN 'CheckedIn' WHEN 5 THEN 'NoShow' END AS "Status", b."CreatedAt"
            FROM "Bookings"."Bookings" b
            WHERE b."HotelId" = @HotelId
              AND (@From IS NULL OR b."CheckOut" > @From)
              AND (@To IS NULL OR b."CheckIn" < @To)
              AND (@Status IS NULL OR CASE b."Status" WHEN 0 THEN 'Pending' WHEN 1 THEN 'Confirmed' WHEN 2 THEN 'Completed' WHEN 3 THEN 'Cancelled' WHEN 4 THEN 'CheckedIn' WHEN 5 THEN 'NoShow' END = @Status)
              AND (@RoomId IS NULL OR b."RoomId" = @RoomId)
            ORDER BY b."CheckIn", b."CreatedAt" DESC
            """;
        return (await connection.QueryAsync<HotelBookingDto>(new CommandDefinition(sql, request, cancellationToken: cancellationToken))).ToList();
    }
}
