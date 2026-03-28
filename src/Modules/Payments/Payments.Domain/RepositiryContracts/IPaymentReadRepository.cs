using Payments.Domain.Entities;

namespace Payments.Domain.RepositiryContracts;

public interface IPaymentReadRepository
{
    Task<Payment> GetByIdAsync(PaymentId paymentId);
}