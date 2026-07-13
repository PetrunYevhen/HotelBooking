using BuildingBlock.Domain;
using MediatR;
using Payments.Application.GatewayContract;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;

namespace Payments.Application.Commands.ConfirmPayment;

public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, Result>
{
    private const string SucceededStatus = "succeeded";

    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayClient _paymentGatewayClient;

    public ConfirmPaymentCommandHandler(IPaymentRepository paymentRepository, IPaymentGatewayClient paymentGatewayClient)
    {
        _paymentRepository = paymentRepository;
        _paymentGatewayClient = paymentGatewayClient;
    }

    public async Task<Result> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);

        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment == null)
            return Result.Failure(Error.NotFound("Payment"));

        if (string.IsNullOrWhiteSpace(payment.ExternalTransactionId))
            return Result.Failure(new Error("Payment.MissingGatewayReference", "Payment has no PaymentIntent attached."));

        var confirmResult = await _paymentGatewayClient.ConfirmPaymentIntentAsync(payment.ExternalTransactionId, request.PaymentMethod, cancellationToken);
        if (confirmResult.IsFailure)
            return Result.Failure(confirmResult.Error);

        if (confirmResult.Value.Status != SucceededStatus)
        {
            return Result.Failure(new Error(
                "Payment.NotCompleted",
                $"Gateway returned non-terminal status '{confirmResult.Value.Status}'."));
        }

        var outcome = payment.Complete(confirmResult.Value.PaymentIntentId);

        if (outcome.IsFailure)
            return outcome;

        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        return Result.Success();
    }
}
