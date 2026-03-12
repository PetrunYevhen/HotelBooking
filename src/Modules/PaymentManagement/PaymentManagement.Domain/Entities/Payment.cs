using BuildingBlock.Domain;
using PaymentManagement.Domain.Entities.Enums;

namespace PaymentManagement.Domain.Entities;

public class Payment :  Entity,  IAggregateRoot
{
    public PaymentId PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public Guid ExternalTransactionId { get; set; }
    
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string FailureReason { get; set; }
     
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
    
    public void Complete(Guid externalTransactionId)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be completed.");

        Status = PaymentStatus.Completed;
        ExternalTransactionId = externalTransactionId;
        CompletedAt = DateTime.UtcNow;

        // Тут ми потім додамо: AddDomainEvent(new PaymentCompletedDomainEvent(Id, BookingId));
    }

    public void Fail(string reason)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be failed.");

        Status = PaymentStatus.Failed;
        FailureReason = reason;

    }
    
}