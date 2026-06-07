using Accommodations.Application.Query.Shared;
using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Rooms.GetRoomFacilities;

public class GetRoomFacilitiesQueryHandler : IRequestHandler<GetRoomFacilitiesQuery, List<FacilityDto>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetRoomFacilitiesQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<FacilityDto>> Handle(GetRoomFacilitiesQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT "RoomFacilityId" AS "Id", "Name", "Category"
                           FROM "Accommodations"."RoomFacilities"
                           WHERE "RoomId" = @RoomId
                           """;

        var parameters = new
        {
            RoomId = request.RoomId
        };

        var query = await connection
            .QueryAsync<FacilityDto>(new CommandDefinition
                (sql, parameters, cancellationToken: cancellationToken));
        
        return query.ToList();    
    }
}