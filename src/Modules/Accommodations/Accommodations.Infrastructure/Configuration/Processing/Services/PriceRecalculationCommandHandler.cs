using Accommodations.Application.Services.Pricing;
using Accommodations.Domain.Entities.Pricing.Enums;
using BuildingBlock.Domain;
using Dapper;
using Infrastructure.Data;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Infrastructure.Configuration.Processing.Services;

public class PriceRecalculationCommandHandler : IRequestHandler<PriceRecalculationCommand, Result>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;
    private readonly IPriceCalculationService _calculationService;

    public PriceRecalculationCommandHandler(INpgsqlConnectionFactory connectionFactory, IPriceCalculationService calculationService)
    {
        _connectionFactory = connectionFactory;
        _calculationService = calculationService;
    }

    public async Task<Result> Handle(PriceRecalculationCommand request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();
        const string selectRoomsSql = """
                                      SELECT "RoomId", "BasePrice_Amount" AS "BasePriceAmount", 
                                             "BasePrice_Currency" AS "BasePriceCurrency", 
                                             "DemandScore"
                                      FROM "Accommodations"."Rooms"
                                      WHERE "IsActive" = true
                                      """;

    const string upsertPricingSql = """
                                    INSERT INTO "Accommodations"."Pricing"
                                        ("PricingId", "RoomId", "Price_Amount", "Price_Currency", "Type", "ValidFrom", "ValidTo", "IsActive")
                                    VALUES
                                        (@PricingId, @RoomId, @PriceAmount, @PriceCurrency, @PricingType, @ValidFrom, @ValidTo, @IsActive)
                                    ON CONFLICT ("RoomId", "ValidFrom", "Type") DO UPDATE
                                    SET "Price_Amount" = EXCLUDED."Price_Amount",
                                        "Price_Currency" = EXCLUDED."Price_Currency"
                                    """;
        
        var rooms = await connection.QueryAsync<RoomRow>(
            new CommandDefinition(selectRoomsSql, cancellationToken: cancellationToken));

        var today = DateTime.UtcNow.Date;
        var records = rooms.SelectMany(room =>
        {
            var basePriceResult = Money.Create(room.BasePriceAmount, room.BasePriceCurrency);
            if (basePriceResult.IsFailure)
                return Enumerable.Empty<UpsertPricingRow>();

            return Enumerable.Range(1, 30).Select(i =>
            {
                var date = today.AddDays(i);
                var price = _calculationService.Calculate(basePriceResult.Value, room.DemandScore, date);
                return new UpsertPricingRow
                {
                    PricingId = Guid.NewGuid(),
                    RoomId = room.RoomId,
                    PriceAmount = price.Amount,
                    PriceCurrency = price.Currency,
                    PricingType = (int)PricingType.Standard,
                    ValidFrom = date,
                    ValidTo = date.AddDays(1),
                    IsActive = true
                };
            }).ToList();
        });
            

        await connection.ExecuteAsync(
            new CommandDefinition(upsertPricingSql, records, cancellationToken: cancellationToken));
        
        
        return Result.Success();
        
    }
    internal sealed class RoomRow
    {
        public Guid RoomId { get; init; }
        public decimal BasePriceAmount { get; init; }
        public string BasePriceCurrency { get; init; }
        public int DemandScore { get; init; }
    }

    private sealed class UpsertPricingRow
    {
        public Guid PricingId { get; init; }
        public Guid RoomId { get; init; }
        public decimal PriceAmount { get; init; }
        public string PriceCurrency { get; init; }
        public int PricingType { get; init; }
        public DateTime ValidFrom { get; init; }
        public DateTime ValidTo { get; init; }
        public bool IsActive { get; init; }
    }
}