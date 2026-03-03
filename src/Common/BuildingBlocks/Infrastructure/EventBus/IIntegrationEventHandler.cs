namespace Infrastructure.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
{
    Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}

public interface IIntegrationEventHandler
{
}