using BuildingBlock.Domain;
using Dapper;
using Infrastructure.Data;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Services.Pricing;

public class PriceCalculationService : IPriceCalculationService
{
    private readonly INpgsqlConnectionFactory _сonnectionFactory;

    public PriceCalculationService(INpgsqlConnectionFactory сonnectionFactory)
    {
        _сonnectionFactory = сonnectionFactory;
    }

    
    public Money Calculate(Money basePrice, int demandScore, DateTime date)
    {
        var amount = Math.Round(
            basePrice.Amount * GetSeasonalMultiplier(date) * GetDemandScoreMultiplier(demandScore), 2);
        return Money.Create(amount, basePrice.Currency).Value;
    }

    private static decimal GetSeasonalMultiplier(DateTime checkIn) => checkIn.Month switch
    {
        6 or 7 or 8 => 1.30m,
        12 when checkIn.Day >= 20 => 1.40m,
        1  when checkIn.Day <= 5 => 1.40m, 
        3 or 4 or 5 or 9 or 10 or 11 => 1.10m,  
        _ => 0.90m,
    };

    private static decimal GetDemandScoreMultiplier(int score) => score switch
    {
        <= 2  => 1.00m,
        <= 5  => 1.15m,
        <= 10 => 1.30m,
        _     => 1.50m,
    };

    private sealed record PriceRow(decimal Amount, string Currency);
    private sealed record RoomRow(decimal Amount, string Currency, int DemandScore);
}