using System.Text.Json.Serialization;
using BuildingBlock.Domain.Events;

namespace BuildingBlock.Domain;

public class Entity
{
    private List<IDomainEvent> _domainEvent;

    [JsonIgnore]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvent.AsReadOnly();
    
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvent.Add(domainEvent);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvent?.Clear();
    }
}