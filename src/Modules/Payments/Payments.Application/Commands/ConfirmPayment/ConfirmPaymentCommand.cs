using BuildingBlock.Domain;
using Payments.Application.Contracts;

namespace Payments.Application.Commands.ConfirmPayment;

public class ConfirmPaymentCommand : CommandBase<Result>
{
    private const string DefaultPaymentMethod = "pm_card_visa";

    public Guid PaymentId { get; }
    public string PaymentMethod { get; }

    public ConfirmPaymentCommand(Guid paymentId, string? paymentMethod = null)
    {
        PaymentId = paymentId;
        PaymentMethod = paymentMethod ?? DefaultPaymentMethod;
    }
}
