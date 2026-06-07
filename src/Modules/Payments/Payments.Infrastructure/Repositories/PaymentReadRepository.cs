using Dapper;
using Infrastructure.Data;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;

namespace Payments.Infrastructure.Repositories;

public class PaymentReadRepository : IPaymentReadRepository
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public PaymentReadRepository(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Payment> GetByIdAsync(PaymentId paymentId) 
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = @"
            SELECT * FROM ""PaymentManagement"".""Payments"" 
            WHERE ""PaymentId"" = @Id";

        var payment = await connection.QuerySingleOrDefaultAsync<Payment>(
            sql, 
            new { Id = paymentId.Value } 
        );

        return payment;
    }
}