using Dapper;
using Infrastructure.Data;
using MediatR;

namespace Payments.Application.Queries.GetPaymenDetails;

public class GetPaymentDetailsQueryHandler : IRequestHandler<GetPaymentDetailsQuery, PaymentDetailsDto>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetPaymentDetailsQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PaymentDetailsDto> Handle(GetPaymentDetailsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = """
                            SELECT "PaymentId", "BookingId", "ExternalTransactionId",
                            "Amount",
                            "Currency",
                            "FailureReason",
                            "Status", "CreatedAt",
                            "CompletedAt"
                            FROM "Payments"."Payments"
                            WHERE "PaymentId" = @PaymentId
                           """;
        var parameters = new
        {
            PaymentId = request.PaymentId
        };
        
        return await connection.QueryFirstOrDefaultAsync<PaymentDetailsDto>(
            new CommandDefinition(sql, parameters,  cancellationToken: cancellationToken));
    }
}