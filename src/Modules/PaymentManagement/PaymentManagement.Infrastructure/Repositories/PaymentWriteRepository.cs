using PaymentManagement.Domain.Entities;
using PaymentManagement.Domain.RepositiryContracts;

namespace PaymentManagement.Infrastructure.Repositories;

public class PaymentWriteRepository : IPaymentWriteRepository
{
    private readonly PaymentDbContext _dbContext;

    public PaymentWriteRepository(PaymentDbContext dbContext)
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