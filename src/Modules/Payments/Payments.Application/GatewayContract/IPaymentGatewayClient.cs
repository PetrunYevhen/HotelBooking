using BuildingBlock.Domain;
using SharedKernel.ValueObjects;

namespace Payments.Application.GatewayContract;

public interface IPaymentGatewayClient
{
    Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(Money amount, string idempotencyKey, CancellationToken cancellationToken);
    Task<Result<PaymentIntentResult>> ConfirmPaymentIntentAsync(string paymentIntentId, string paymentMethod, CancellationToken cancellationToken);
    Task<Result<RefundResult>> RefundPaymentAsync(string paymentIntentId, Money amount, string idempotencyKey, CancellationToken cancellationToken);
}

public sealed record PaymentIntentResult(string PaymentIntentId, string Status);
public sealed record RefundResult(string RefundId, string Status);
