using BuildingBlock.Domain.Events;

namespace Infrastructure.DomainEventDispatching;

public interface IDomainEventAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents(); 
    void ClearAllDomainEvents();
}