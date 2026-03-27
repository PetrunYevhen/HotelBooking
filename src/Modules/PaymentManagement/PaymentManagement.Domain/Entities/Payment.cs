using BuildingBlock.Domain;
using PaymentManagement.Domain.Entities.Events;
using PaymentManagement.Domain.Enums;

namespace PaymentManagement.Domain.Entities;

public class Payment :  Entity,  IAggregateRoot
{
    public PaymentId PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public Guid ExternalTransactionId { get; set; }
    
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? FailureReason { get; set; } = string.Empty;
     
    public DateTime CreatedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    
    public PaymentStatus Status { get; set; }
    
    
    public Payment() { }
    
    public Payment(PaymentId id, Guid bookingId, decimal amount, string currency)
    {
        PaymentId = id;
        BookingId = bookingId;
        Amount = amount;
        Currency = currency;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public static Payment CreatePayment(PaymentId paymentId, Guid bookingId, decimal amount, string currency)
    {
        var payment = new Payment(paymentId, bookingId, amount, currency);
        
        payment.AddDomainEvent(new PaymentCreatedDomainEvent(paymentId, bookingId, amount, currency));
        
        return payment; 
    }
    
    public void Complete(Guid paymentId)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be completed.");

        Status = PaymentStatus.Completed;
        // ExternalTransactionId = externalTransactionId;
        CompletedAt = DateTime.UtcNow;

        AddDomainEvent(new PaymentCompletedDomainEvent(
            paymentId,
            this.BookingId,
            this.Amount,
            this.Currency,
            this.CompletedAt
            ));
    }

    public void Fail(string reason)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be failed.");

        Status = PaymentStatus.Failed;
        FailureReason = reason;

    }
    
}