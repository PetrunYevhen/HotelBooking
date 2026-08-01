using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Accommodations.Application.Query.HotelAddOns;

public sealed class GetHotelAddOnsQueryHandler : IRequestHandler<GetHotelAddOnsQuery, IReadOnlyList<HotelAddOnDto>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;
    public GetHotelAddOnsQueryHandler(INpgsqlConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IReadOnlyList<HotelAddOnDto>> Handle(GetHotelAddOnsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();
        const string sql = """
            SELECT "HotelAddOnId", "HotelId", "Code", "Name", "Description",
                   "Price_Amount" AS "PriceAmount", "Price_Currency" AS "PriceCurrency",
                   "PricingType", "IsActive"
            FROM "Accommodations"."HotelAddOns"
            WHERE "HotelId" = @HotelId
              AND (@IncludeInactive OR "IsActive" = true)
            ORDER BY "Name"
            """;
        var rows = await connection.QueryAsync<HotelAddOnRow>(new CommandDefinition(sql,
            new { request.HotelId, request.IncludeInactive }, cancellationToken: cancellationToken));
        return rows.Select(Map).ToList();
    }

    internal static HotelAddOnDto Map(HotelAddOnRow x) => new()
    {
        HotelAddOnId = x.HotelAddOnId,
        HotelId = x.HotelId,
        Code = x.Code,
        Name = x.Name,
        Description = x.Description,
        PriceAmount = x.PriceAmount,
        PriceCurrency = x.PriceCurrency,
        PricingType = x.PricingType,
        IsActive = x.IsActive
    };
}

internal sealed class HotelAddOnRow
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
