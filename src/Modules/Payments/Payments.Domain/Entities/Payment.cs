using BuildingBlock.Domain;
using Payments.Domain.Entities.Enums;
using Payments.Domain.Entities.Events;
using SharedKernel.ValueObjects;

namespace Payments.Domain.Entities;

public class Payment :  Entity,  IAggregateRoot
{
    public PaymentId PaymentId { get; private set; }
    public Guid BookingId { get; private set; }
    public string ExternalTransactionId { get; private set; }
    
    public Money TotalAmount { get; private set; }
    public string? FailureReason { get; private set; } = string.Empty;
     
    public DateTime CreatedAt { get; private set; }
    public DateTime CompletedAt { get; private set; }
    
    public PaymentStatus Status { get; private set; }
    
    public Payment() { }
    
    private Payment(Guid bookingId, Money totalAmount)
    {
        PaymentId = PaymentId.New();
        BookingId = bookingId;
        TotalAmount = totalAmount;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new PaymentCreatedDomainEvent(PaymentId, BookingId, TotalAmount));
    }

    public static Result<Payment> Create(Guid bookingId, Money totalAmount)
    {
        if (bookingId == Guid.Empty)
            return Result.Failure<Payment>(new Error("Payment.InvalidBooking", "BookingId cannot be empty."));

        if (totalAmount is null)
            return Result.Failure<Payment>(new Error("Payment.InvalidAmount", "Amount is required."));
        
        return Result.Success(new Payment(bookingId, totalAmount));
    }
    
    public Result Complete(string externalTransactionId)
    {
        if (Status != PaymentStatus.Pending)
            return Result.Failure(new Error("Payment.InvalidStatus", $"Cannot complete payment in status '{Status}'."));

        if (string.IsNullOrWhiteSpace(externalTransactionId))
            return Result.Failure(new Error("Payment.InvalidTransaction", "External transaction ID is required."));

        Status = PaymentStatus.Completed;
        ExternalTransactionId = externalTransactionId;
        CompletedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentCompletedDomainEvent(PaymentId.Value, BookingId, TotalAmount, CompletedAt));

        return Result.Success();
    }

    public Result Fail(string reason)
    {
        if (Status != PaymentStatus.Pending)
            return Result.Failure(new Error("Payment.InvalidStatus", $"Cannot fail payment in status '{Status}'."));

        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure(new Error("Payment.InvalidReason", "Failure reason is required."));

        Status = PaymentStatus.Failed;
        FailureReason = reason;

        AddDomainEvent(new PaymentFailedDomainEvent(PaymentId, BookingId, reason));

        return Result.Success();
    }

    public Result Refund()
    {
        if (Status != PaymentStatus.Completed)
            return Result.Failure(new Error("Payment.InvalidStatus", $"Cannot refund payment in status '{Status}'."));

        Status = PaymentStatus.Refunded;

        AddDomainEvent(new PaymentRefundedDomainEvent(PaymentId, BookingId, TotalAmount));

        return Result.Success();
    }
    
}