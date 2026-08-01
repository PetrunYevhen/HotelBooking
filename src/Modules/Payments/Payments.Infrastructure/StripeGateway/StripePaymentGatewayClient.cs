using BuildingBlock.Domain;
using Payments.Application.GatewayContract;
using SharedKernel.ValueObjects;
using Stripe;

namespace Payments.Infrastructure.StripeGateway;

public class StripePaymentGatewayClient : IPaymentGatewayClient
{
    private readonly IStripeClient _stripeClient;

    public StripePaymentGatewayClient(IStripeClient stripeClient)
    {
        _stripeClient = stripeClient;
    }

    public async Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(Money amount, string idempotencyKey, CancellationToken cancellationToken)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = ToMinorUnits(amount.Amount),
                Currency = amount.Currency.ToLowerInvariant(),
                PaymentMethodTypes = new List<string> { "card" }
            };

            var requestOptions = new RequestOptions { IdempotencyKey = idempotencyKey };

            var service = new PaymentIntentService(_stripeClient);
            var intent = await service.CreateAsync(options, requestOptions, cancellationToken);

            return Result.Success(new PaymentIntentResult(intent.Id, intent.Status));
        }
        catch (StripeException ex)
        {
            return Result.Failure<PaymentIntentResult>(new Error("PaymentGateway.CreateFailed", ex.Message));
        }

    }

    public async Task<Result<PaymentIntentResult>> ConfirmPaymentIntentAsync(string paymentIntentId, string paymentMethod, CancellationToken cancellationToken)
    {
        try
        {
            var options = new PaymentIntentConfirmOptions
            {
                PaymentMethod = paymentMethod
            };

            var service = new PaymentIntentService(_stripeClient);
            var intent = await service.ConfirmAsync(paymentIntentId, options, cancellationToken: cancellationToken);

            return Result.Success(new PaymentIntentResult(intent.Id, intent.Status));
        }
        catch (StripeException ex)
        {
            return Result.Failure<PaymentIntentResult>(new Error("PaymentGateway.ConfirmFailed", ex.Message));
        }
    }

    public async Task<Result<RefundResult>> RefundPaymentAsync(string paymentIntentId, Money amount, string idempotencyKey, CancellationToken cancellationToken)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
                Amount = ToMinorUnits(amount.Amount)
            };

            var service = new RefundService(_stripeClient);
            var refund = await service.CreateAsync(options, new RequestOptions { IdempotencyKey = idempotencyKey }, cancellationToken);

            return Result.Success(new RefundResult(refund.Id, refund.Status));
        }
        catch (StripeException ex)
        {
            return Result.Failure<RefundResult>(new Error("PaymentGateway.RefundFailed", ex.Message));
        }
    }

    internal static long ToMinorUnits(decimal amount) =>
        Convert.ToInt64(Math.Round(amount * 100m, MidpointRounding.AwayFromZero));
}
