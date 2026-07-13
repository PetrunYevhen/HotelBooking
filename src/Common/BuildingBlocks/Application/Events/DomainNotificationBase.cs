namespace Application.Events;

public class DomainNotificationBase<T> : IDomainEventNotification<T>
{
    public Guid Id { get; }
    public T DomainEvent { get; }
    
    public DomainNotificationBase(T domainEvent, Guid id)
    {
        Id = id;
        DomainEvent = domainEvent;
    }

    
}