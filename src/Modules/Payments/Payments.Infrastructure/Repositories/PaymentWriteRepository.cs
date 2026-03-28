using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;

namespace Payments.Infrastructure.Repositories;

public class PaymentWriteRepository : IPaymentWriteRepository
{
    private readonly PaymentsDbContext _dbContext;

    public PaymentWriteRepository(PaymentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Payment payment)
    {
        await _dbContext.AddAsync(payment);
        await _dbContext.SaveChangesAsync();

        return payment.PaymentId.Value;
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken)
    {
        _dbContext.Update(payment);
        await _dbContext.SaveChangesAsync();
    }
}