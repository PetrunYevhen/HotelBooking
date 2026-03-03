namespace Infrastructure.DomainEventDispatching;

public interface IDomainEventDispatcher
{
    Task DispatchEventAsync();
}