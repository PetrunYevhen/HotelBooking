using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Bookings.Application.Query.GetBookingUserId;

public sealed class GetBookingUserIdQueryHandler(INpgsqlConnectionFactory connectionFactory) : IRequestHandler<GetBookingUserIdQuery, Guid?>
{
    public async Task<Guid?> Handle(GetBookingUserIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateNewConnection();
        return await connection.QueryFirstOrDefaultAsync<Guid?>(new CommandDefinition(
            "SELECT \"UserId\" FROM \"Bookings\".\"Bookings\" WHERE \"BookingId\" = @BookingId",
            new { request.BookingId }, cancellationToken: cancellationToken));
    }
}
