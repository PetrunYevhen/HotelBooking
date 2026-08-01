using Accommodations.Application.ClientContracts;
using Accommodations.Application.Query.Hotels.GetAllHotels;
using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.Hotels.SearchHotels;

public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, List<HotelDto>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;
    private readonly IBookingsClient _bookingsClient;

    public SearchHotelsQueryHandler(INpgsqlConnectionFactory connectionFactory, IBookingsClient bookingsClient)
    {
        _connectionFactory = connectionFactory;
        _bookingsClient = bookingsClient;
    }

    public async Task<List<HotelDto>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT r."RoomId", r."HotelId", r."Capacity",
                                   h."Name", h."City", h."Country", h."Rating",
                                   h."MinRoomPrice_Amount" AS "MinRoomPriceAmount",
                                   h."MinRoomPrice_Currency" AS "MinRoomPriceCurrency"
                            FROM "Accommodations"."Rooms" r
                            JOIN "Accommodations"."Hotels" h ON h."HotelId" = r."HotelId"
                            WHERE r."IsActive" = true AND h."Status" = 1
                              AND r."Capacity" >= @MinCapacityPerRoom
                              AND (@Destination IS NULL OR h."City" ILIKE @Destination OR h."Country" ILIKE @Destination)
                            """;

        var minCapacityPerRoom = (int)Math.Ceiling((double)request.Guests / request.Rooms);
        var destinationPattern = string.IsNullOrWhiteSpace(request.Destination) ? null : $"%{request.Destination}%";

        var parameters = new
        {
            MinCapacityPerRoom = minCapacityPerRoom,
            Destination = destinationPattern
        };

        var candidates = (await connection.QueryAsync<CandidateRoomRow>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken))).ToList();

        if (candidates.Count == 0)
            return new List<HotelDto>();

        if (request.CheckIn.HasValue && request.CheckOut.HasValue)
        {
            var candidateRoomIds = candidates.Select(c => c.RoomId).Distinct().ToList();
            var occupiedRoomIds = await _bookingsClient.GetOverlappingRoomIdsAsync(
                candidateRoomIds, request.CheckIn.Value, request.CheckOut.Value, cancellationToken);

            candidates = candidates.Where(c => !occupiedRoomIds.Contains(c.RoomId)).ToList();
        }

        return candidates
            .GroupBy(c => c.HotelId)
            .Where(g => g.Count() >= request.Rooms)
            .Select(g => g.First())
            .Select(c => new HotelDto
            {
                HotelId = c.HotelId,
                Name = c.Name,
                City = c.City,
                Country = c.Country,
                Rating = c.Rating,
                MinRoomPriceAmount = c.MinRoomPriceAmount,
                MinRoomPriceCurrency = c.MinRoomPriceCurrency
            })
            .ToList();
    }

    private sealed class CandidateRoomRow
    {
        public Guid RoomId { get; set; }
        public Guid HotelId { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double? Rating { get; set; }
        public decimal? MinRoomPriceAmount { get; set; }
        public string? MinRoomPriceCurrency { get; set; }
    }
}
