using BuildingBlock.Domain;
using Payments.Application.Contracts;

namespace Payments.Application.Commands.RefundPayment;

public class RefundPaymentCommand : CommandBase<Result>
{
    public Guid BookingId { get; }
    public decimal Amount { get; }
    public string Currency { get; }

    public RefundPaymentCommand(Guid bookingId, decimal amount, string currency)
    {
        BookingId = bookingId;
        Amount = amount;
        Currency = currency;
    }
}
