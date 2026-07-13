using BuildingBlock.Domain;
using MediatR;
using Payments.Application.GatewayContract;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;
using SharedKernel.ValueObjects;

namespace Payments.Application.Commands.CreatePayment;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayClient _paymentGatewayClient;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IPaymentGatewayClient paymentGatewayClient)
    {
        _paymentRepository = paymentRepository;
        _paymentGatewayClient = paymentGatewayClient;
    }

    public async Task<Result> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var existingPayment = await _paymentRepository.GetByBookingIdAsync(request.BookingId, cancellationToken);
        if (existingPayment is not null)
            return Result.Success();

        var priceResult = Money.Create(request.TotalAmount, request.Currency);
        if (priceResult.IsFailure)
            return Result.Failure(new Error("Payment.InvalidAmount", $"Invalid payment amount: {request.TotalAmount} {request.Currency}."));

        var idempotencyKey = $"booking:{request.BookingId}:payment-intent:v1";
        var intentResult = await _paymentGatewayClient.CreatePaymentIntentAsync(priceResult.Value, idempotencyKey, cancellationToken);
        if (intentResult.IsFailure)
            return Result.Failure(new Error("Payment.GatewayError", $"Failed to create payment intent: {intentResult.Error.Message}"));

        var paymentResult = Payment.Create(request.BookingId, priceResult.Value);
        if (paymentResult.IsFailure) 
            return Result.Failure(new Error("Payment.CreateFailed", $"Failed to create payment for booking {request.BookingId}."));
        
        var attachResult = paymentResult.Value.AttachGatewayReference(intentResult.Value.PaymentIntentId);
        if (attachResult.IsFailure)
            return Result.Failure(attachResult.Error);
        
        await _paymentRepository.AddAsync(paymentResult.Value, cancellationToken);
        return Result.Success();
    }
}