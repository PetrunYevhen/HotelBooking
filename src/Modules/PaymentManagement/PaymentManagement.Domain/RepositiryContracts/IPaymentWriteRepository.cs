using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Domain.RepositiryContracts;

public interface IPaymentWriteRepository
{
    Task<Guid> AddAsync(Payment payment);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken);
}