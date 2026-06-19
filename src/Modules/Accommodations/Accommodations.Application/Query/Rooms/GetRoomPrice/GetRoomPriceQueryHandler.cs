using Accommodations.Application.Services.Pricing;
using BuildingBlock.Domain;
using Dapper;
using Infrastructure.Data;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Query.Rooms.GetRoomPrice;

public class GetRoomPriceQueryHandler : IRequestHandler<GetRoomPriceQuery, Result<Money>>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetRoomPriceQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<Money>> Handle(GetRoomPriceQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                           SELECT "Price_Amount" AS "PriceAmount", 
                                  "Price_Currency" AS "PriceCurrency"
                           FROM "Accommodations"."Pricing"
                           WHERE "RoomId" = @RoomId
                             AND "ValidFrom" <= @CheckIn AND "ValidTo" >= @CheckIn
                             AND "IsActive" = true
                           ORDER BY "Type" DESC
                           LIMIT 1
                           """;

        var parameters = new
        {
            RoomId = request.RoomId,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut
        };
        var row = await connection.QueryFirstOrDefaultAsync<PriceRow>
            (new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        
        

        var moneyResult = Money.Create(row.PriceAmount, row.PriceCurrency);
        return moneyResult.IsFailure
            ? Result.Failure<Money>(moneyResult.Error)
            : Result.Success(moneyResult.Value);
    }
}

internal sealed class PriceRow
{
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; }
}
                
            