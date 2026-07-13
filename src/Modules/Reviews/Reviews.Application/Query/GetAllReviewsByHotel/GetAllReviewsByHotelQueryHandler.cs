using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Reviews.Application.Query.GetAllReviewsByHotel;

public class GetAllReviewsByHotelQueryHandler : IRequestHandler<GetAllReviewsByHotelQuery, List<ReviewDto>>
{
    private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

    public GetAllReviewsByHotelQueryHandler(INpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task<List<ReviewDto>> Handle(GetAllReviewsByHotelQuery request, CancellationToken cancellationToken)
    {
        using var connection = _npgsqlConnectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT "ReviewId", "UserId",
                            "Rating", "Title",
                            "Comment", "PublishedAt", "IsBookingVerified"
                            FROM "Reviews"."Reviews"
                            WHERE "HotelId" = @HotelId
                           """;

        var parameters = new
        {
            HotelId = request.HotelId
        };

        var query = await connection.QueryAsync<ReviewDto>
            (new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        
        return query.ToList();
    }
}