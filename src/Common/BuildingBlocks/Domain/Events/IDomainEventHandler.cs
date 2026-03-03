using MediatR;

namespace BuildingBlock.Domain.Events;

public interface IDomainEventHandler<in T> : INotificationHandler<T>
    where T : IDomainEvent;