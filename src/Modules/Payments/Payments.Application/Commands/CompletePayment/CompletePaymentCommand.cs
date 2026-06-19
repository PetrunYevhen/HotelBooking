using BuildingBlock.Domain;
using Payments.Application.Contracts;

namespace Payments.Application.Commands.CompletePayment;

public class CompletePaymentCommand : CommandBase<Result>
{
    public Guid PaymentId { get; set; }
    public string ExternalTransactionId { get; set; }
    public CompletePaymentCommand(Guid paymentId, string externalTransactionId)
    {
        PaymentId = paymentId;
        ExternalTransactionId = externalTransactionId;
    }
}