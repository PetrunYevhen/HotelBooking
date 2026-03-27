using Dapper;
using Infrastructure.Data;
using PaymentManagement.Domain.Entities;
using PaymentManagement.Domain.RepositiryContracts;

namespace PaymentManagement.Infrastructure.Repositories;

public class PaymentReadRepository : IPaymentReadRepository
{
    private readonly INpgsqlConnectionFactory _connectionFactory;

    public PaymentReadRepository(INpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Payment> GetByIdAsync(PaymentId paymentId) 
    {
        using var connection = _connectionFactory.CreateConnection();

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