// using Infrastructure.EventBus;
// using MediatR;
// using PaymentManagement.Application.Events;
//
// namespace PaymentManagement.Application.DomainEventHandlers;
//
// public class PaymentCreatedNotificationHandler : INotificationHandler<PaymentCreatedNotification>
// {
//     private readonly IEventBus _eventBus;
//
//     public PaymentCreatedNotificationHandler(IEventBus eventBus)
//     {
//         _eventBus = eventBus;
//     }
//
//     public async Task Handle(PaymentCreatedNotification notification, CancellationToken cancellationToken)
//     {
//         await _eventBus.Publish(new PaymentCreatedIntegrationEvent(
//             notification.PaymentId,))
//     }
// }