using Infrastructure.DomainEventDispatching;

namespace Notifications.Infrastructure.Configuration.Processing;

public class NullDomainEventDispatcher : IDomainEventDispatcher
{
    public Task DispatchEventAsync() => Task.CompletedTask;
}