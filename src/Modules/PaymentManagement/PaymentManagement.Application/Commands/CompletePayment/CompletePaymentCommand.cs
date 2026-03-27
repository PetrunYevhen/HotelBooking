using PaymentManagement.Application.Contracts;

namespace PaymentManagement.Application.Commands.CompletePayment;

public class CompletePaymentCommand : CommandBase
{
    public Guid PaymentId { get; set; }
    // public Guid ExternalPaymentId { get; set; }
    public CompletePaymentCommand(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}