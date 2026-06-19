using Payments.Domain.Entities.Enums;

namespace Payments.Application.Queries.GetPaymenDetails;

public class PaymentDetailsDto
{
    public Guid PaymentId { get; init; }
    public Guid BookingId { get; init; }
    public string ExternalTransactionId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public string? FailureReason { get; init; }
    public PaymentStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    
}