using Accommodations.Application.Query.Shared;
using Accommodations.Domain.Entities.Hotels;
using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.GetHotelFacilities;

public class GetFacilitiesQueryHandler : IRequestHandler<GetHotelFacilitiesQuery, List<FacilityDto>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetFacilitiesQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<FacilityDto>> Handle(GetHotelFacilitiesQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT "HotelFacilityId" AS "Id", "Name", "Category"
                           FROM "Accommodations"."HotelFacilities"
                           WHERE "HotelId" = @HotelId
                           """;

        var parameters = new
        {
            HotelId = request.HotelId
        };

        var query = await connection
            .QueryAsync<FacilityDto>(new CommandDefinition
                (sql, parameters, cancellationToken: cancellationToken));
        
        return query.ToList();
    }
}