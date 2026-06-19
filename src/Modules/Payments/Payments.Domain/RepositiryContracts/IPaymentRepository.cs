using Payments.Domain.Entities;

namespace Payments.Domain.RepositiryContracts;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(PaymentId paymentId, CancellationToken cancellationToken);
    Task<Guid> AddAsync(Payment payment, CancellationToken cancellationToken);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken);
}