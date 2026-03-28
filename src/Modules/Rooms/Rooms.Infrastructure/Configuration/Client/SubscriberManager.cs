using Autofac;
using Infrastructure.Client;
using Serilog;

namespace Rooms.Infrastructure.Configuration.Client;

public class SubscriberManager : IStartable
{
    private readonly IClient _client;
    private readonly ILogger _logger;
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IEnumerable<ISubscription> _subscriptions;

    public SubscriberManager(IClient client, ILogger logger, ILifetimeScope lifetimeScope, IEnumerable<ISubscription> subscriptions)
    {
        _client = client;
        _logger = logger;
        _lifetimeScope = lifetimeScope;
        _subscriptions = subscriptions;
    }

    public void Start()
    {
        _logger.Information("Starting subscribers for Room Module...");

        foreach (var subscription in _subscriptions)
        {
            subscription.Subscride(_client, _lifetimeScope);
        }
        
        _logger.Information("Activated {Count} subscriptions", _subscriptions.Count());
    }
}