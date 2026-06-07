using MediatR;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;

namespace Payments.Application.Commands.CompletePayment;

public class CompletePaymentCommandHandler : IRequestHandler<CompletePaymentCommand>
{
    private readonly IPaymentWriteRepository _paymentWriteRepository;
    private readonly IPaymentReadRepository _paymentReadRepository;

    public CompletePaymentCommandHandler(
        IPaymentWriteRepository paymentWriteRepository, 
        IPaymentReadRepository paymentReadRepository)
    {
        _paymentWriteRepository = paymentWriteRepository;
        _paymentReadRepository = paymentReadRepository;
    }

    public async Task Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        
        var payment = await _paymentReadRepository.GetByIdAsync(paymentId);
        if (payment == null)
            throw new InvalidOperationException($"Payment {request.PaymentId} not found.");

        payment.Complete();

        
        await _paymentWriteRepository.UpdateAsync(payment, cancellationToken);
    }
}