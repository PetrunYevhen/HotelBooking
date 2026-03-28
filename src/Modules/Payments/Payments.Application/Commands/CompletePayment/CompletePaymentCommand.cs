using Payments.Application.Contracts;

namespace Payments.Application.Commands.CompletePayment;

public class CompletePaymentCommand : CommandBase
{
    public Guid PaymentId { get; set; }
    // public Guid ExternalPaymentId { get; set; }
    public CompletePaymentCommand(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}