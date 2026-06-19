using Microsoft.EntityFrameworkCore;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;

namespace Payments.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentsDbContext _dbContext;

    public PaymentRepository(PaymentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payment?> GetByIdAsync(PaymentId paymentId, CancellationToken cancellationToken)
    {
        return await 
            _dbContext.Payments.FirstOrDefaultAsync
                (p => p.PaymentId == paymentId, cancellationToken);
    }

    public async Task<Guid> AddAsync(Payment payment,  CancellationToken cancellationToken)
    {
        await _dbContext.Payments.AddAsync(payment, cancellationToken);

        return payment.PaymentId.Value;
    }

    public Task UpdateAsync(Payment payment, CancellationToken cancellationToken)
    {
        _dbContext.Payments.Update(payment);
        return Task.CompletedTask;
    }
}