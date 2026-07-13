using BuildingBlock.Domain;
using Payments.Application.GatewayContract;
using SharedKernel.ValueObjects;
using Stripe;

namespace Payments.Infrastructure.StripeGateway;

/// <summary>
/// HTTP-contract adapter for stripe-mock. stripe-mock returns canned statuses, so deterministic
/// test outcomes live here and never affect the production Stripe adapter.
/// </summary>
public sealed class StripeMockPaymentGatewayClient : IPaymentGatewayClient
{
    private const string DeclinedPaymentMethod = "pm_card_visa_chargeDeclined";
    private readonly IStripeClient _stripeClient;

    public StripeMockPaymentGatewayClient(IStripeClient stripeClient)
    {
        _stripeClient = stripeClient;
    }

    public async Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(
        Money amount,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = StripePaymentGatewayClient.ToMinorUnits(amount.Amount),
                Currency = amount.Currency.ToLowerInvariant(),
                PaymentMethodTypes = ["card"]
            };
            var requestOptions = new RequestOptions { IdempotencyKey = idempotencyKey };
            var intent = await new PaymentIntentService(_stripeClient)
                .CreateAsync(options, requestOptions, cancellationToken);

            return Result.Success(new PaymentIntentResult(intent.Id, intent.Status));
        }
        catch (StripeException exception)
        {
            return Result.Failure<PaymentIntentResult>(
                new Error("PaymentGateway.CreateFailed", exception.Message));
        }
    }

    public async Task<Result<PaymentIntentResult>> ConfirmPaymentIntentAsync(
        string paymentIntentId,
        string paymentMethod,
        CancellationToken cancellationToken)
    {
        try
        {
            var options = new PaymentIntentConfirmOptions { PaymentMethod = paymentMethod };
            var intent = await new PaymentIntentService(_stripeClient)
                .ConfirmAsync(paymentIntentId, options, cancellationToken: cancellationToken);
            var status = paymentMethod == DeclinedPaymentMethod
                ? "requires_payment_method"
                : "succeeded";

            return Result.Success(new PaymentIntentResult(intent.Id, status));
        }
        catch (StripeException exception)
        {
            return Result.Failure<PaymentIntentResult>(
                new Error("PaymentGateway.ConfirmFailed", exception.Message));
        }
    }
}
