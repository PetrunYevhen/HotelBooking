using Infrastructure.EventBus;
using MediatR;
using Payments.Application.Events;
using Payments.IntegrationEvents;

namespace Payments.Application.DomainEventHandlers;

public class PaymentCompletedNotificationHandler : INotificationHandler<PaymentCompletedNotification>
{
    private readonly IEventBus _eventBus;

    public PaymentCompletedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(PaymentCompletedNotification notification, CancellationToken cancellationToken)
    {
        await _eventBus.Publish(new PaymentCompletedIntegrationEvent(
            notification.Id,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.PaymentId,
            notification.DomainEvent.BookingId
            ), cancellationToken);
        
    }
}