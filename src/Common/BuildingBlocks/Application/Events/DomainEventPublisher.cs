using BuildingBlock.Domain.Events;
using MediatR;

namespace Application.Events;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;
    private readonly List<IDomainEvent> _events = [];
    
    public DomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();
    
    // Add the event to the list and publish it using MediatR
    public void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void AddEventRange(IEnumerable<IDomainEvent> events)
    {
        _events.AddRange(events);
    }

    public async Task HandleEvents()
    {
        foreach (var @event in _events)
        {
            await _mediator.Publish(@event);
        }
    }
}