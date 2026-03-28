using Bookings.IntegrationEvents;
using MediatR;
using Payments.Domain.Entities;
using Payments.Domain.RepositiryContracts;

namespace Payments.Application.IntegrationEventHandlers;

public class BookingCreatedIntegrationEventHandler : INotificationHandler<BookingCreatedIntegrationEvent>
{
    private readonly IPaymentWriteRepository _paymentWriteRepository;

    public BookingCreatedIntegrationEventHandler(IPaymentWriteRepository paymentWriteRepository)
    {
        _paymentWriteRepository = paymentWriteRepository;
    }

    public async Task Handle(BookingCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(Guid.NewGuid());
        var newPayment = Payment.CreatePayment(
            paymentId,
            notification.BookingId,
            notification.TotalPrice,
            notification.Currency
        );
        
        await _paymentWriteRepository.AddAsync(newPayment);
    }
}