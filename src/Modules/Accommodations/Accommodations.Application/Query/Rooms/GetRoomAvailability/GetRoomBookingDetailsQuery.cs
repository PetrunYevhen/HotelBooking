using Accommodations.Application.Contracts;
using Dapper;
using Infrastructure.Data;
using MediatR;
using SharedKernel.Contracts;

namespace Accommodations.Application.Query.Rooms.GetRoomAvailability;

public sealed class GetRoomBookingDetailsQuery(Guid roomId) : QueryBase<RoomBookingDetails?>
{
    public Guid RoomId { get; } = roomId;
}

public sealed class GetRoomBookingDetailsQueryHandler(INpgsqlConnectionFactory connections)
    : IRequestHandler<GetRoomBookingDetailsQuery, RoomBookingDetails?>
{
    public async Task<RoomBookingDetails?> Handle(GetRoomBookingDetailsQuery request, CancellationToken cancellationToken)
    {
        using var connection = connections.CreateNewConnection();
        return await connection.QuerySingleOrDefaultAsync<RoomBookingDetails>(new CommandDefinition(
            """SELECT "RoomId", "HotelId", "Capacity", "IsActive" FROM "Accommodations"."Rooms" WHERE "RoomId" = @RoomId""",
            new { request.RoomId }, cancellationToken: cancellationToken));
    }
}
