using BuildingBlock.Domain;
using Payments.Application.Contracts;

namespace Payments.Application.Commands.CreatePayment;

public class CreatePaymentCommand : CommandBase<Result>
{
    public CreatePaymentCommand(Guid bookingId, decimal totalAmount, string currency)
    {
        BookingId = bookingId;
        TotalAmount = totalAmount;
        Currency = currency;
    }

    public Guid BookingId { get;  }
    public decimal TotalAmount { get; }
    public string Currency { get; }
}