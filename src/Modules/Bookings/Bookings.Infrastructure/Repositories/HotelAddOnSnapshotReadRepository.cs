using Bookings.Application.Services.AddOns;
using Bookings.Domain.Entities;
using Bookings.Domain.Entities.Enums;
using BuildingBlock.Domain;
using Dapper;
using Infrastructure.Data;
using SharedKernel.ValueObjects;

namespace Bookings.Infrastructure.Repositories;

public sealed class HotelAddOnSnapshotReadRepository : IHotelAddOnSnapshotReader
{
    private readonly INpgsqlConnectionFactory _connectionFactory;
    public HotelAddOnSnapshotReadRepository(INpgsqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<HotelAddOnSnapshot?> GetByIdAsync(Guid hotelAddOnId, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();
        const string sql = """
            SELECT "HotelAddOnId", "HotelId", "Code", "Name", "Description",
                   "Price_Amount" AS "PriceAmount", "Price_Currency" AS "PriceCurrency",
                   "PricingType", "IsActive"
            FROM "Bookings"."HotelAddOnSnapshots"
            WHERE "HotelAddOnId" = @HotelAddOnId
            """;
        var row = await connection.QuerySingleOrDefaultAsync<SnapshotRow>(new CommandDefinition(sql,
            new { HotelAddOnId = hotelAddOnId }, cancellationToken: cancellationToken));
        if (row is null)
            return null;

        var price = Money.Create(row.PriceAmount, row.PriceCurrency);
        if (price.IsFailure || !Enum.IsDefined(typeof(HotelAddOnPricingType), row.PricingType))
            return null;
        var snapshot = HotelAddOnSnapshot.Create(row.HotelAddOnId, row.HotelId, row.Code, row.Name,
            row.Description, price.Value, (HotelAddOnPricingType)row.PricingType, row.IsActive);
        return snapshot.IsSuccess ? snapshot.Value : null;
    }

    private sealed class SnapshotRow
    {
        public Guid HotelAddOnId { get; init; }
        public Guid HotelId { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public decimal PriceAmount { get; init; }
        public string PriceCurrency { get; init; } = string.Empty;
        public int PricingType { get; init; }
        public bool IsActive { get; init; }
    }
}
