namespace BuildingBlock.Domain.Events;

public interface IDomainEventPublisher
{
    IReadOnlyCollection<IDomainEvent> Events { get; }

    void AddEvent(IDomainEvent @event);
    void AddEventRange(IEnumerable<IDomainEvent> events);
    Task HandleEvents();
}