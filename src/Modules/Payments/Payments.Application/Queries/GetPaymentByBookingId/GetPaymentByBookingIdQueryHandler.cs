using Dapper;
using Infrastructure.Data;
using MediatR;
using Payments.Application.Queries.GetPaymenDetails;

namespace Payments.Application.Queries.GetPaymentByBookingId;

public class GetPaymentByBookingIdQueryHandler : IRequestHandler<GetPaymentByBookingIdQuery, PaymentDetailsDto>
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public GetPaymentByBookingIdQueryHandler(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PaymentDetailsDto> Handle(GetPaymentByBookingIdQuery request, CancellationToken cancellationToken)
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
                            WHERE "BookingId" = @BookingId
                           """;

        return await connection.QueryFirstOrDefaultAsync<PaymentDetailsDto>(
            new CommandDefinition(sql, new { request.BookingId }, cancellationToken: cancellationToken));
    }
}
