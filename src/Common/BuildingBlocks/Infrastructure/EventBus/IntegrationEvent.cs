using MediatR;

namespace Infrastructure.EventBus;

public abstract class IntegrationEvent : INotification 
{
    public Guid Id { get; set; }
    public DateTime OccuredOn { get; set; }

    protected IntegrationEvent(Guid id, DateTime occuredOn)
    {
        Id = id;
        OccuredOn = occuredOn;
    }
    
}