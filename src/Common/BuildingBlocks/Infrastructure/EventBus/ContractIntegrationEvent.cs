namespace Infrastructure.EventBus;

public sealed class ContractIntegrationEvent<T>(Guid id, DateTime occurredOn, T data) : IntegrationEvent(id, occurredOn)
{
    public T Data { get; } = data;
}
