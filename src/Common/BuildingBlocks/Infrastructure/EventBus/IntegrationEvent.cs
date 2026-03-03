using MediatR;

namespace Infrastructure.EventBus;

public abstract class IntegrationEvent : INotification 
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    
    protected IntegrationEvent(Guid id, DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }
    
}