using BuildingBlock.Domain;
using MediatR;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;

namespace Payments.Application.Commands.CompletePayment;

public class CompletePaymentCommandHandler : IRequestHandler<CompletePaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;

    public CompletePaymentCommandHandler(
        IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        
        var payment = await _paymentRepository.GetByIdAsync(paymentId,  cancellationToken);
        if (payment == null)
            throw new InvalidOperationException($"Payment {request.PaymentId} not found.");

        var result = payment.Complete(request.ExternalTransactionId);
        if (result.IsFailure)
            return result;

        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        return Result.Success();
    }
}