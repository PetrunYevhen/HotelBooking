using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.GetHotelOwner;

public sealed class GetHotelOwnerQueryHandler(INpgsqlConnectionFactory connectionFactory) : IRequestHandler<GetHotelOwnerQuery, Guid?>
{
    public async Task<Guid?> Handle(GetHotelOwnerQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateNewConnection();
        return await connection.QueryFirstOrDefaultAsync<Guid?>(new CommandDefinition(
            "SELECT \"OwnerUserId\" FROM \"Accommodations\".\"Hotels\" WHERE \"HotelId\" = @HotelId",
            new { request.HotelId }, cancellationToken: cancellationToken));
    }
}
