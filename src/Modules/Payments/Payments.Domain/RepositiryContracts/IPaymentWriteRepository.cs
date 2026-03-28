using Payments.Domain.Entities;

namespace Payments.Domain.RepositiryContracts;

public interface IPaymentWriteRepository
{
    Task<Guid> AddAsync(Payment payment);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken);
}