using Autofac;

namespace Infrastructure.Client;

public interface ISubscription
{
    void Subscride(IClient client, ILifetimeScope lifetimeScope);
}