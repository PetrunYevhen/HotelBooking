using BuildingBlock.Domain;
using MediatR;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;
using SharedKernel.ValueObjects;

namespace Payments.Application.Commands.CreatePayment;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var priceResult = Money.Create(request.TotalAmount, request.Currency);
        if (priceResult.IsFailure)
            return Result.Failure(new Error("Payment.InvalidAmount", $"Invalid payment amount: {request.TotalAmount} {request.Currency}."));
        
        var paymentResult = Payment.Create(request.BookingId, priceResult.Value);
        if (paymentResult.IsFailure) 
            return Result.Failure(new Error("Payment.CreateFailed", $"Failed to create payment for booking {request.BookingId}."));
        
        await _paymentRepository.AddAsync(paymentResult.Value, cancellationToken);
        return Result.Success();
    }
}