using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Domain.RepositiryContracts;

public interface IPaymentReadRepository
{
    Task<Payment> GetByIdAsync(PaymentId paymentId);
}