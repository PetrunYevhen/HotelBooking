using BuildingBlock.Domain;
using System.Globalization;
using MediatR;
using Payments.Application.GatewayContract;
using Payments.Domain.Entities.Enums;
using Payments.Domain.RepositiryContracts;
using SharedKernel.ValueObjects;

namespace Payments.Application.Commands.RefundPayment;

public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayClient _paymentGatewayClient;

    public RefundPaymentCommandHandler(IPaymentRepository paymentRepository, IPaymentGatewayClient paymentGatewayClient)
    {
        _paymentRepository = paymentRepository;
        _paymentGatewayClient = paymentGatewayClient;
    }

    public async Task<Result> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByBookingIdAsync(request.BookingId, cancellationToken);
        if (payment is null)
            return Result.Failure(Error.NotFound("Payment"));

        if (payment.Status != PaymentStatus.Completed)
            return Result.Success();

        var amountResult = Money.Create(request.Amount, request.Currency);
        if (amountResult.IsFailure)
            return Result.Failure(amountResult.Error);

        // The same cancellation integration event can be delivered again after a failed commit.
        // Keep this key stable so the gateway returns the original refund instead of charging twice.
        var amountKey = amountResult.Value.Amount.ToString("0.00", CultureInfo.InvariantCulture);
        var idempotencyKey = $"refund:{payment.PaymentId.Value:N}:{amountKey}:{amountResult.Value.Currency}";
        var refundResult = await _paymentGatewayClient.RefundPaymentAsync(
            payment.ExternalTransactionId!, amountResult.Value, idempotencyKey, cancellationToken);
        if (refundResult.IsFailure)
            return Result.Failure(refundResult.Error);

        var outcome = payment.Refund(amountResult.Value);
        if (outcome.IsFailure)
            return outcome;

        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        return Result.Success();
    }
}
